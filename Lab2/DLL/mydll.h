#pragma once

#ifdef DLL_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

struct header
{
	int addr;
	int size;
};

extern "C"
{
	DLL_API void MapSendMessage(int addr, const wchar_t* str);
	DLL_API std::wstring MapReceiveMessage(header& h);
}