#include "session.h"
#include "mydll.h"

int i = 0;
std::vector<Session*> sessions;

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
				//SafeWrite(L"сессия ", session->sessionID, L", сообщение \"", m.data, L"\"");
				session->saveMessage(m);
				break;
			}
			}
		}
	}
	return;
}

void processClient(tcp::socket s)
{
	try
	{
		while (true)
		{
			header h;
			std::wstring str = ReceiveM(s, h);

			switch (h.type)
			{
			case INIT:
			{
				SafeWrite(L"Новый клиент подключился");
				break;
			}
			case EXIT:
			{
				SafeWrite(L"Клиент отключился");
				SendM(s, CONFIRM);
				return;
			}
			case START:
			{
				for (int j = 0; j < h.num; ++j)
				{
					sessions.push_back(new Session(i++));
					std::thread(MyThread, sessions.back()).detach();
				}
				break;
			}
			case SEND:
			{
				if (h.size > 0)
				{
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
				}
				break;
			}
			case STOP:
			{
				if (i > 0)
				{
					sessions.back()->addMessage(MT_CLOSE);
					sessions.pop_back();
					--i;
				}
				break;
			}
			default:
				break;
			}
			SendM(s, CONFIRM, i);
		}
	}
	catch (std::exception& e)
	{
		std::wcerr << "Exception: " << e.what() << std::endl;
	}
}


int main()
{
	std::locale::global(std::locale(".1251"));
	std::wcin.imbue(std::locale());
	std::wcout.imbue(std::locale());
	std::wcerr.imbue(std::locale());

	try
	{
		int port = 12345;
		boost::asio::io_context io;
		tcp::acceptor a(io, tcp::endpoint(tcp::v4(), port));

		while (true)
		{
			std::thread(processClient, a.accept()).detach();
		}
	}
	catch (std::exception& e)
	{
		std::wcerr << "Exception: " << e.what() << std::endl;
	}
	
	return 0;
}