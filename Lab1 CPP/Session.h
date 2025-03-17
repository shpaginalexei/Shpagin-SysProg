﻿#pragma once
#include "SysProg.h"

enum MessageTypes
{
	MT_CLOSE,
	MT_DATA,
};

struct MessageHeader
{
	int messageType;
	int size;
};

struct Message
{
	MessageHeader header = { 0 };
	std::wstring data;
	Message() = default;
	Message(MessageTypes messageType, const std::wstring& data = L"")
		:data(data)
	{
		header = { messageType, int(data.length()) };
	}
};

class Session
{
	std::queue<Message> messages;
	std::mutex mtx;
	std::condition_variable cv;

	std::mutex f_mtx;

public:
	int sessionID;

	Session(int sessionID) 
		:sessionID(sessionID)
	{
	}

	~Session() = default;

	void addMessage(Message& m)
	{
		std::lock_guard<std::mutex> lg(mtx);
		messages.push(m);
		cv.notify_one();
	}

	bool getMessage(Message& m)
	{
		bool res = false;
		std::unique_lock<std::mutex> ul(mtx);
		cv.wait(ul, [this] { return !messages.empty(); });
		if (!messages.empty())
		{
			res = true;
			m = messages.front();
			messages.pop();
		}
		return res;
	}

	void addMessage(MessageTypes messageType, const std::wstring& data = L"")
	{
		Message m(messageType, data);
		addMessage(m);
	}

	void saveMessage(const Message& message)
	{
		// можно сделать через fstream
		std::wstring filePath = L"files\\" + std::to_wstring(sessionID) + L".txt";
		HANDLE hFile = CreateFile(filePath.c_str(), GENERIC_READ | GENERIC_WRITE,
			FILE_SHARE_WRITE | FILE_SHARE_READ, NULL, OPEN_ALWAYS, 0, 0);
		SetFilePointer(hFile, 0, NULL, FILE_END);

		std::wstring str = message.data + L"\n";
		const wchar_t* data = str.c_str();
		DWORD dataSize = (DWORD)(str.size() * sizeof(wchar_t));
		DWORD bytesWritten = 0;
		WriteFile(hFile, data, dataSize, &bytesWritten, NULL);

		CloseHandle(hFile);
	}
};
