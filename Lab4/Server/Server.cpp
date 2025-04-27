#include "SafeWrite.h"
#include "message.h"
#include "session.h"

#include <map>
#include <thread>

int maxID = MR_USER;
std::map<int, std::shared_ptr<Session>> sessions;

void processClient(tcp::socket s)
{
	try
	{
		while (true)
		{
			Message m;
			int code = m.receive(s);
			SafeWrite(m.header.to, ": ", m.header.from, ": ", m.header.type);
			switch (code)
			{
			case MT_INIT:
			{
				auto session = std::make_shared<Session>(++maxID, m.data);
				sessions[session->id] = session;
				Message::send(s, session->id, MR_BROKER, MT_INIT);

				for (auto& [id, ses] : sessions)
				{
					if (id != session->id)
					{
						Message mes = Message(id, session->id, MT_INIT);
						ses->add(mes);
						mes = Message(session->id, id, MT_INIT);
						sessions[session->id]->add(mes);
					}
				}
				break;
			}
			case MT_EXIT:
			{
				sessions.erase(m.header.from);
				Message::send(s, m.header.from, MR_BROKER, MT_CONFIRM);

				for (auto& [id, ses] : sessions)
				{
					if (id != m.header.from)
					{
						Message mes = Message(id, m.header.from, MT_EXIT);
						ses->add(mes);
					}
				}
				return;
			}
			case MT_GETDATA:
			{
				auto iSession = sessions.find(m.header.from);
				if (iSession != sessions.end())
				{
					iSession->second->send(s);
				}
				break;
			}
			default:
			{
				auto iSessionFrom = sessions.find(m.header.from);
				if (iSessionFrom != sessions.end())
				{
					auto iSessionTo = sessions.find(m.header.to);
					if (iSessionTo != sessions.end())
					{
						iSessionTo->second->add(m);
					}
					else if (m.header.to == MR_ALL)
					{
						for (auto& [id, session] : sessions)
						{
							if (id != m.header.from)
								session->add(m);
						}
					}
					else if (m.header.to == MR_BROKER)
					{
						SafeWrite("������� �����, ��������� \"", m.data, "\"");
					}
					Message::send(s, m.header.from, MR_BROKER, MT_CONFIRM);
				}
				break;
			}
			}
		}
	}
	catch (std::exception& e)
	{
		std::wcerr << "Exception: " << e.what() << endl;
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