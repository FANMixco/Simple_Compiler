var a = 0;
var b = 0;
var c = 0;
print "El mayor de 3 numeros";
print "Ingrese el numero a:";
read_int a;
print "Ingrese el numero b:";
read_int b;
print "Ingrese el numero c:";
read_int c;
if a > b then
	if a > c then
		print "El numero a es el mayor";
	else
		print "El numero c es el mayor";
	end;
else
	if b > c then
		print "El numero b es el mayor";
	else
		print "El numero c es el mayor";
	end;
end;