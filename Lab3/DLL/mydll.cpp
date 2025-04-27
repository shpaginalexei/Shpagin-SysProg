#include "pch.h"
#include "mydll.h"

boost::asio::io_context io;
tcp::socket* MySocket = nullptr;

void SendM(tcp::socket& s, int type, int num, int addr, const wchar_t* str)
{
	header h;
	if (str != nullptr)
		h = { type, num, addr, (int)(wcslen(str) * sizeof(wchar_t)) };
	else
		h = { type, num, addr, 0 };

	sendData(s, &h);
	if (h.size)
		sendData(s, str, h.size);
}

std::wstring ReceiveM(tcp::socket& s, header& h)
{
	receiveData(s, &h);
	std::wstring str;
	if (h.size)
	{
		str.resize(h.size / sizeof(wchar_t));
		receiveData(s, str.data(), h.size);
	}
	return str;
}

int SendM_C(int type, int num, int addr, const wchar_t* str)
{
	if ((MessageType)type == INIT)
	{
		MySocket = new tcp::socket(io);
		tcp::resolver r(io);
		boost::asio::connect(*MySocket, r.resolve("127.0.0.1", "12345"));
	}

	SendM(*MySocket, type, num, addr, str);

	header hConfirm = { 0 };
	receiveData(*MySocket, &hConfirm);

	if ((MessageType)type == EXIT)
	{
		delete MySocket;
	}

	if (hConfirm.type == CONFIRM)
		return hConfirm.num;
	else
		return -1;
}

void ReceiveM_C(header& h)
{
	receiveData(*MySocket, &h);
}
