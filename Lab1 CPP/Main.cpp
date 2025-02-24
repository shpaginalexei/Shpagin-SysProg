#include "Session.h"

HANDLE hEvent;

DWORD WINAPI MyThread(LPVOID lpParameter)
{
	auto session = static_cast<Session*>(lpParameter);
	SafeWrite("session", session->sessionID, "created");
	while (true)
	{
		Message m;
		if (session->getMessage(m))
		{
			switch (m.header.messageType)
			{
			case MT_CLOSE:
			{
				SafeWrite("session", session->sessionID, "closed");
				delete session;
				return 0;
			}
			/*case MT_DATA:
			{
				SafeWrite("session", session->sessionID, "data", m.data);
				Sleep(500 * session->sessionID);
				break;
			}*/
			}
		}
	}
	return 0;
}

void start()
{
	InitializeCriticalSection(&cs);

	vector<Session*> sessions;

	HANDLE hStartEvent = CreateEvent(NULL, FALSE, FALSE, L"StartEvent");
	HANDLE hStopEvent = CreateEvent(NULL, TRUE, FALSE, L"StopEvent");
	HANDLE hConfirmEvent = CreateEvent(NULL, FALSE, FALSE, L"ConfirmEvent");
	HANDLE hExitEvent = CreateEvent(NULL, FALSE, FALSE, L"ExitEvent");
	HANDLE hControlEvents[3] = { hStartEvent, hStopEvent, hExitEvent };

	int i = 0;
	SetEvent(hConfirmEvent);

	do
	{
		int n = WaitForMultipleObjects(3, hControlEvents, FALSE, INFINITE) - WAIT_OBJECT_0;
		switch (n)
		{
		case 0:
			sessions.push_back(new Session(i++));
			CloseHandle(CreateThread(NULL, 0, MyThread, (LPVOID)sessions.back(), 0, NULL));
			SetEvent(hConfirmEvent);
			break;
		case 1:
			sessions.back()->addMessage(MT_CLOSE);
			sessions.pop_back();
			--i;
			SetEvent(hConfirmEvent);
			break;
		case 2:
			SetEvent(hConfirmEvent);
			return;
		}
	} while (i);
	SetEvent(hConfirmEvent);
	DeleteCriticalSection(&cs);
}

int main()
{
	start();
	return 0;
}