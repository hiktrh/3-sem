.586P											
.MODEL FLAT, STDCALL									
includelib kernel32.lib									

ExitProcess PROTO : DWORD								
MessageBoxA PROTO : DWORD, : DWORD, : DWORD, : DWORD	

.STACK 4096								

.CONST									

ZERO		EQU			0
MB_OK		EQU			0

.DATA									

myBytes		BYTE		10h, 20h, 30h, 40h
myWords		WORD		8Ah, 3Bh, 44h, 5Fh, 99h
myDoubles	DWORD		1, 2, 3, 4, 5, 6
myPointer	DWORD		myDoubles
result		DB			"Результат", 0
message0	DB			"В массиве есть нулевой элемент", 0
message1	DB			"В массиве нет нулевого элемента", 0
myArray		DWORD		5, 7, 4, 2, 6, 7, 3

.CODE									

main PROC 								

	mov ESI, 0
	mov EAX, myDoubles[ESI]		
	mov EDX, [myDoubles + ESI]	 

	
	mov ESI, offset myArray				
	mov ECX, lengthof myArray			
	mov EAX, 0						

CYCLE:
	add EAX, [ESI]						
	add	ESI, type myArray		
	loop CYCLE					

	
	mov ESI, offset myArray				
	mov ECX, lengthof myArray
	mov EBX, 1						

CHECK_ZERO:
	cmp dword ptr [ESI], 0 
	je CASE_ZERO 
	add ESI, type myArray 
	loop CHECK_ZERO
	
	jmp DISPLAY_RESULT

CASE_ZERO:
	mov EBX, 0

DISPLAY_RESULT:
	
	cmp EBX, 0
	jne NO_ZERO
	
	
	invoke MessageBoxA, 0, offset message0, offset result, MB_OK
	jmp ENDING

NO_ZERO:

	invoke MessageBoxA, 0, offset message1, offset result, MB_OK

ENDING:
	push 0								
	call ExitProcess					

main ENDP								

end main