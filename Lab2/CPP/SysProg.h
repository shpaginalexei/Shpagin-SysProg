#pragma once

#include <windows.h>
#include <iostream>
#include <vector>
#include <queue>
#include <string>
#include <thread>
#include <mutex>
#include <condition_variable>


inline void DoWrite()
{
	std::wcout << std::endl;
}

template <class T, typename... Args> inline void DoWrite(T& value, Args... args)
{
	std::wcout << value;
	DoWrite(args...);
}

std::mutex m;
template <typename... Args> inline void SafeWrite(Args... args)
{
	std::lock_guard<std::mutex> lg(m);
	DoWrite(args...);
}