.586P
.MODEL FLAT, STDCALL
includelib kernel32.lib

ExitProcess PROTO : DWORD
MessageBoxA PROTO : DWORD, : DWORD, : DWORD, : DWORD

.STACK 4096

.CONST

.DATA

INT_VAL 	 SDWORD 10
UINT_LIT 	 DWORD 13

.CODE
main PROC
mov 	 EAX, type INT_VAL
mov 	 EBX, type UINT_LIT
push 0
call ExitProcess
main ENDP
end main
