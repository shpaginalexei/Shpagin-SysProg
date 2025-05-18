#pragma once

#include "message.h"
#include <queue>
#include <mutex>
#include <chrono>

class Session
{
public:
	int id;
	std::wstring name;
	std::queue<Message> messages;

	std::mutex mx;
	Session(int id, std::wstring name)
		:id(id), name(name)
	{
	}

	void add(Message& m)
	{
		std::lock_guard<std::mutex> lg(mx);
		messages.push(m);
	}

	void send(tcp::socket& s)
	{
		std::lock_guard<std::mutex> lg(mx);
		if (messages.empty())
		{
			Message::send(s, id, MR_BROKER, MT_NODATA);
		}
		else
		{
			messages.front().send(s);
			messages.pop();
		}
	}
};