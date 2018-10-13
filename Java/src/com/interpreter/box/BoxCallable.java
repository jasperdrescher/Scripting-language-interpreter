package com.interpreter.box;

import java.util.List;

interface BoxCallable {     
	int arity();
	  Object call(Interpreter interpreter, List<Object> arguments);
}
