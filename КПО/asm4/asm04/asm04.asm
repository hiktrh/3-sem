.586P
.MODEL FLAT, STDCALL
includelib kernel32.lib
ExitProcess PROTO : DWORD
.STACK 4096
.DATA

INT_VAL 	 SDWORD 10
UINT_LIT 	 DWORD 13
.CODE
main PROC
mov EAX, INT_VAL
mov EBX, UINT_LIT
push 0
call ExitProcess
main ENDP
END main
