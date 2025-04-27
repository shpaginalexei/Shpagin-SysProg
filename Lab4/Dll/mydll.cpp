#include "pch.h"
#include "mydll.h"

boost::asio::io_context io;
tcp::socket* MySocket = nullptr; // shared_ptr ???

// Конвертация std::wstring в wchar_t* (с выделением памяти)
wchar_t* copy_wstring(const std::wstring& str)
{
	if (str.empty())
	{
		return nullptr;
	}
	std::size_t size = (str.length() + 1) * sizeof(wchar_t);
	wchar_t* buffer = (wchar_t*)malloc(size);
	if (buffer) {
		memcpy(buffer, str.c_str(), size);
	}
	return buffer;
}

MessageTransfer SendMsg(int to, int type, const wchar_t* data)
{
	if ((MessageTypes)type == MT_INIT)
	{
		MySocket = new tcp::socket(io);
		tcp::resolver r(io);
		boost::asio::connect(*MySocket, r.resolve("127.0.0.1", "12345"));
	}

	Message m(to, Message::clientID, type, data);
	m.send(*MySocket);
	if (m.receive(*MySocket) == MT_INIT)
	{
		Message::clientID = m.header.to;
	}

	if ((MessageTypes)type == MT_EXIT)
	{
		delete MySocket;
	}

	MessageTransfer result = {};
	result.header = m.header;
	result.data = copy_wstring(m.data);
	result.clientID = m.clientID;

	return result;
}

void FreeMessageTransfer(MessageTransfer msg)
{
	if (msg.data)
	{
		free((void*)msg.data);
	}
}