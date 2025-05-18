import curses
import socket
import sys
import textwrap
import threading
import time

from message import *


@dataclass
class User:
    id: int

    def name(self) -> str:
        if self.id == 10:
            return f"Главный поток"
        elif self.id == 50:
            return f"Всем потокам"
        else:
            return f"Пользователь {self.id}"


class PythonClient:
    def __init__(self, host: str, port: int):
        self.HOST = host
        self.PORT = port

        self.my_id: int = None
        self.running: bool = None
        self.other_ids: list[int] = [10, 50]
        self.history: list[Message] = []

        self._socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.connect()

    def process_messages(self):
        while self.running:
            m = Message.SendMessage(
                self._socket, MessageRecipient.MR_BROKER, MessageType.MT_GETDATA
            )
            if m.Header.Type == MessageType.MT_DATA:
                self.history.append(m)
            elif m.Header.Type == MessageType.MT_INIT:
                self.other_ids.append(m.Header.From)
            elif m.Header.Type == MessageType.MT_EXIT:
                self.other_ids.remove(m.Header.From)
            else:
                time.sleep(1)

    def connect(self):
        self._socket.connect((self.HOST, self.PORT))

        m = Message.SendMessage(
            self._socket, MessageRecipient.MR_BROKER, MessageType.MT_INIT
        )
        if m.Header.Type != MessageType.MT_INIT:
            raise Exception()

        self.my_id = Message.ClientID
        print(f"Твой clientID: {self.my_id}")

        self.running = True
        t = threading.Thread(target=self.process_messages)
        t.start()

    def draw_users(
        self,
        selected_idx: int,
        offset: int,
        height: int,
        width: int,
    ) -> curses.window:
        user_win = curses.newwin(height, width, 0, 0)
        user_win.box()
        visible = height - 2
        for idx in range(visible):
            uidx = idx + offset
            if uidx >= len(self.other_ids):
                break
            user = User(self.other_ids[uidx])
            attr = curses.A_REVERSE if uidx == selected_idx else curses.A_NORMAL
            user_win.addnstr(idx + 1, 1, user.name().ljust(width - 2), width - 2, attr)
        user_win.refresh()
        return user_win

    def draw_history_panel(
        self,
        height: int,
        width: int,
        start_y: int,
        start_x: int,
        offset: int = 0,
    ) -> curses.window:
        hist_win = curses.newwin(height, width, start_y, start_x)
        hist_win.box()
        visible = height - 2
        wrapped_lines = []

        for m in self.history:
            msg_prefix = f"{m.Header.From} → {m.Header.To}: "
            full_msg = msg_prefix + m.Data
            wrap_width = width - 2
            wrapped = textwrap.wrap(full_msg, wrap_width)
            for i, line in enumerate(wrapped):
                wrapped_lines.append(line)

        display_lines = wrapped_lines[-visible - offset : len(wrapped_lines) - offset]

        for i, msg in enumerate(display_lines):
            msg_space = width - 2
            hist_win.addnstr(i + 1, 1, msg.ljust(msg_space), msg_space)

        hist_win.refresh()

        return hist_win

    def draw_input_box(self, max_y: int, max_x: int) -> curses.window:
        box_y = max_y - 3
        box_win = curses.newwin(3, max_x, box_y, 0)
        box_win.box()
        box_win.refresh()
        input_win = curses.newwin(1, max_x - 2, box_y + 1, 1)
        input_win.keypad(True)
        input_win.scrollok(True)
        input_win.idlok(True)
        input_win.nodelay(True)
        input_win.erase()
        input_win.move(0, 0)
        return input_win

    def run(self, stdscr: curses.window):
        self.history.append(
            Message(0, 0, MessageType.MT_DATA, f"Твой ID: {self.my_id}")
        )

        curses.use_default_colors()
        curses.curs_set(1)
        stdscr.nodelay(False)

        selected_idx = 0
        user_offset = 0
        input_buffer = []
        text = ""

        while True:
            stdscr.erase()
            max_y, max_x = stdscr.getmaxyx()

            users = [User(i) for i in self.other_ids]

            # Compute panel sizes
            user_w = (
                max(20, max(len(u.name()) for u in users) + 2)
                if len(users) != 0
                else 20
            )
            hist_h = max_y - 3
            hist_w = max_x - user_w

            visible_users = hist_h - 2
            if selected_idx < user_offset:
                user_offset = selected_idx
            elif selected_idx >= user_offset + visible_users:
                user_offset = selected_idx - visible_users + 1

            self.draw_users(selected_idx, user_offset, hist_h, user_w)
            self.draw_history_panel(hist_h, hist_w, 0, user_w)

            input_win = self.draw_input_box(max_y, max_x)
            try:
                key = input_win.get_wch()
            except curses.error:
                key = None

            if key == "\x1b":
                m = Message.SendMessage(
                    self._socket, MessageRecipient.MR_BROKER, MessageType.MT_EXIT
                )
                self.running = False
                break
            elif key == "\t":
                selected_idx = (selected_idx + 1) % len(users)
            elif key == curses.KEY_BTAB:
                selected_idx = (selected_idx - 1) % len(users)
            elif key in ("\n", "\r"):
                text = "".join(input_buffer)
                m = Message(
                    self.other_ids[selected_idx], self.my_id, MessageType.MT_DATA, text
                )
                self.history.append(m)
                m.Send(self._socket)
                input_buffer = []
            elif key in (curses.KEY_BACKSPACE, "\b", "\x7f"):
                if input_buffer:
                    input_buffer.pop()
            elif isinstance(key, str) and key.isprintable():
                input_buffer.append(key)

            input_win.erase()
            input_win.addstr("".join(input_buffer)[-max_x + 4 :])
            input_win.refresh()

            # stdscr.refresh()
            time.sleep(0.05)

        stdscr.erase()
        stdscr.refresh()


def main():
    if len(sys.argv) > 1:
        HOST = sys.argv[1]
    else:
        HOST = "localhost"
    PORT = 12345

    app = PythonClient(HOST, PORT)
    curses.wrapper(app.run)


if __name__ == "__main__":
    main()
