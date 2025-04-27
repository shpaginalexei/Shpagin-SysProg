#pragma once

#ifdef DLL_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

#include "message.h"

struct MessageTransfer
{
	MessageHeader header = {};
	const wchar_t* data = nullptr;
	int clientID = 0;
};

extern "C"
{
	DLL_API MessageTransfer SendMsg(int to, int type = MT_DATA, const wchar_t* data = nullptr);
	DLL_API void FreeMessageTransfer(MessageTransfer msg);
}
