#define ASM_HEAD \
<< ".586P" << endl \
<< ".MODEL FLAT, STDCALL" << endl \
<< "includelib kernel32.lib" << endl \
<< "ExitProcess PROTO : DWORD" << endl \
<< ".STACK 4096" << endl \
<< ".DATA" << endl << endl

#define ASM_FOOTER \
<< ".CODE" << endl \
<< "main PROC" << endl \
<< "mov EAX, INT_VAL" << endl \
<< "mov EBX, UINT_LIT" << endl \
<< "push 0" << endl \
<< "call ExitProcess" << endl \
<< "main ENDP" << endl \
<< "END main" << endl
