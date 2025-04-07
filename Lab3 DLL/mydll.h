#pragma once

#ifdef LAB3DLL_EXPORTS
#define LAB3DLL_API __declspec(dllexport)
#else
#define LAB3DLL_API __declspec(dllimport)
#endif			

#include "asio.h"
#include <string>

enum MessageType
{
	INIT,
	EXIT,
	START,
	SEND,
	STOP,
	CONFIRM,
};

struct header
{
	int type;
	int num;
	int addr;
	int size;
};

extern "C"
{
	LAB3DLL_API void SendM(tcp::socket& s, int type, int num = 0, int addr = 0, const wchar_t* str = nullptr);
	LAB3DLL_API std::wstring ReceiveM(tcp::socket&s, header& h);

	LAB3DLL_API int SendM_C(int type, int num = 0, int addr = 0, const wchar_t* str = nullptr);
	LAB3DLL_API void ReceiveM_C(header&h);
}