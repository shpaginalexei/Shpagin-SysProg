#pragma once

#ifdef LAB2DLL_EXPORTS
#define LAB2DLL_API __declspec(dllexport)
#else
#define LAB2DLL_API __declspec(dllimport)
#endif

struct header
{
	int addr;
	int size;
};

extern "C"
{
	LAB2DLL_API void MapSendMessage(int addr, const wchar_t* str);
	LAB2DLL_API std::wstring MapReceiveMessage(header& h);
}