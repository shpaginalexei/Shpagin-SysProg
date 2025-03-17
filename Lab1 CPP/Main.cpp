#include <locale.h>
#include "session.h"
#include "mydll.h"

void MyThread(Session* session)
{
	SafeWrite(L"сессия ", session->sessionID, L" создана");
	while (true)
	{
		Message m;
		if (session->getMessage(m))
		{
			switch (m.header.messageType)
			{
			case MT_CLOSE:
			{
				SafeWrite(L"сессия ", session->sessionID, L" закрыта");
				delete session;
				return;
			}
			case MT_DATA:
			{
				session->saveMessage(m);
				break;
			}
			}
		}
	}
	return;
}

void start()
{
	int i = 0;
	std::vector<Session*> sessions;

	HANDLE hStartEvent = CreateEvent(NULL, FALSE, FALSE, L"StartEvent");
	HANDLE hDataEvent = CreateEvent(NULL, FALSE, FALSE, L"DataEvent");
	HANDLE hStopEvent = CreateEvent(NULL, FALSE, FALSE, L"StopEvent");
	HANDLE hConfirmEvent = CreateEvent(NULL, FALSE, FALSE, L"ConfirmEvent");
	HANDLE hExitEvent = CreateEvent(NULL, FALSE, FALSE, L"ExitEvent");

	HANDLE hControlEvents[4] = { hStartEvent, hDataEvent, hStopEvent, hExitEvent };

	SetEvent(hConfirmEvent);
	while (i >= 0)
	{
		int n = WaitForMultipleObjects(4, hControlEvents, FALSE, INFINITE) - WAIT_OBJECT_0;
		switch (n)
		{
		case 0:
		{
			sessions.push_back(new Session(i++));
			std::thread t(MyThread, sessions.back());
			t.detach();
			SetEvent(hConfirmEvent);
			break;
		}
		case 1:
		{
			header h;
			std::wstring str = MapReceiveMessage(h);

			switch (h.addr)
			{
			case -2:
			{
				SafeWrite(L"Главный поток, сообщение \"", str, L"\"");
				for (const auto& session : sessions)
				{
					session->addMessage(MT_DATA, str);
				}
				break;
			}
			case -1:
			{
				SafeWrite(L"Главный поток, сообщение \"", str, L"\"");
				break;
			}
			default:
			{
				if (h.addr < sessions.size())
				{
					sessions[h.addr]->addMessage(MT_DATA, str);
				}
			}
			};

			SetEvent(hConfirmEvent);
			break;
		}
		case 2:
		{
			if (i == 0)
			{
				SetEvent(hExitEvent);
				break;
			}
			sessions.back()->addMessage(MT_CLOSE);
			sessions.pop_back();
			--i;
			SetEvent(hConfirmEvent);
			break;
		}
		case 3:
		{
			for (auto session : sessions)
				delete session;
			SetEvent(hConfirmEvent);
			return;
		}
		}
	};

	SetEvent(hConfirmEvent);
}

int main()
{
#ifdef _WIN32
	std::wcin.imbue(std::locale("rus_rus.866"));
	std::wcout.imbue(std::locale("rus_rus.866"));
#else
	std::wcin.imbue(std::locale("ru_RU.UTF-8"));
	std::wcout.imbue(std::locale("ru_RU.UTF-8"));
#endif

	start();

	return 0;
}