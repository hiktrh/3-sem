.586P
.MODEL FLAT, STDCALL
includelib kernel32.lib
includelib user32.lib

ExitProcess PROTO : DWORD
MessageBoxA PROTO : DWORD, : DWORD, : DWORD, : DWORD

.STACK 4096

.CONST

.DATA
MB_OK EQU 0
a DB 2
b DB 4
STR1 DB "СУММА", 0	
RESULT DB "Результат сложения = < >", 0
HW DD ?

.CODE

main PROC
START:

	mov al, a
	add al, b
	add al, 30h
	mov RESULT+22, al
	INVOKE MessageBoxA, HW, OFFSET RESULT, OFFSET STR1, MB_OK

	push -1
	call ExitProcess
main ENDP

end main