package com.interpreter.box;

import java.util.List;               
import java.util.Map;                

class BoxClass implements BoxCallable {                
  final String name;  
  final BoxClass superclass;
  private final Map<String, BoxFunction> methods;

  BoxClass(String name, BoxClass superclass,  
          Map<String, BoxFunction> methods) {
   this.superclass = superclass;
    this.name = name;                                      
    this.methods = methods;                                
  }           
  
  BoxFunction findMethod(BoxInstance instance, String name) {
	    if (methods.containsKey(name)) {                         
	    	return methods.get(name).bind(instance);                              
	    }
	    
	    if (superclass != null) {                      
	        return superclass.findMethod(instance, name);
	      }
	    
	    return null;                                             
	  }

  @Override                          
  public String toString() {         
    return name;                     
  }      
  
  @Override                                                            
  public Object call(Interpreter interpreter, List<Object> arguments) {
    BoxInstance instance = new BoxInstance(this);       
    BoxFunction initializer = methods.get("init");                     
    if (initializer != null) {                                         
      initializer.bind(instance).call(interpreter, arguments);         
    }
    
    if (isInitializer) return closure.getAt(0, "this");
    
    return instance;                                                   
  }

  @Override                                                            
  public int arity() {                                                 
	  BoxFunction initializer = methods.get("init");
	    if (initializer == null) return 0;            
	    return initializer.arity();                                                         
  }
}
