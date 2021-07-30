grammar FlatBuffers ;

// Parser rules

schema : include* ( namespace_decl | type_decl | enum_decl | union_decl | root_decl | file_extension_decl | file_identifier_decl | attribute_decl | rpc_decl )* ;

include : 'include' STRING_CONSTANT ';' ;

namespace_decl : 'namespace' IDENT ( '.' IDENT )* ';' ;

attribute_decl : 'attribute' STRING_CONSTANT ';' ;

type_decl : ( 'table' | 'struct' ) IDENT metadata '{' ( field_decl )* '}' ;

enum_decl : 'enum' IDENT ':' integer_type_name metadata '{' commasep_enumval_decl '}' ;

union_decl : 'union' IDENT metadata '{' commasep_unionval_decl '}' ;

unionval_decl : ( IDENT ':' )? type ;

commasep_unionval_decl : unionval_decl ( ',' unionval_decl )* ','? ;

root_decl : 'root_type' IDENT ';' ;

field_decl : IDENT ':' type ( '=' defaultValue_decl )? metadata ';' ;

defaultValue_decl : scalar | 'null' | IDENT ;

rpc_decl : 'rpc_service' IDENT metadata '{' rpc_method+ '}' ;

rpc_method : IDENT '(' type ')' ':' type metadata ';' ;

type : vector_type | structvector_type | core_type ;
core_type : base_type_name | ns_ident ;
vector_type : '[' core_type ']' ;
structvector_type : '[' core_type ':' INTEGER_CONSTANT ']' ; // should this also accept hex numbers?

enumval_decl : IDENT ( '=' integer_const )? ;

commasep_enumval_decl : enumval_decl ( ',' enumval_decl )* ','? ;

// Legacy bug: this should really only be STRING_CONSTANT.
// However, FlatSharp has not enforced this historically,
// so we keep the definition of acceptable metadata values
// quite broad for backwards compatibility.
// https://github.com/jamescourtney/FlatSharp/issues/186
metadata_value : scalar | STRING_CONSTANT | IDENT;

metadata_item : Key=IDENT ( ':' Value=metadata_value )? ;

metadata_list : metadata_item ( ',' metadata_item )* ;

metadata : ( '(' metadata_list ')' )? ;

scalar : integer_const | boolean_constant | FLOAT_CONSTANT ;

file_extension_decl : 'file_extension' STRING_CONSTANT ';' ;

file_identifier_decl : 'file_identifier' STRING_CONSTANT ';' ;

ns_ident : IDENT ( '.' IDENT )* ;

integer_const :  INTEGER_CONSTANT | HEX_INTEGER_CONSTANT ;

integer_type_name : 'byte' 
                  | 'ubyte'
                  | 'short'
                  | 'ushort'
                  | 'int'
                  | 'uint'
                  | 'long'
                  | 'ulong'
                  | 'int8'
                  | 'uint8'
                  | 'int16'
                  | 'uint16'
                  | 'int32'
                  | 'uint32'
                  | 'int64'
                  | 'uint64'
                  ;

base_type_name : integer_type_name 
               | 'bool'
               | 'float'
               | 'double'
               | 'float32'
               | 'float64'
               | 'string'
               ;

boolean_constant : 'true' | 'false' ;

// Lexer rules
STRING_CONSTANT : '"' ~["\r\n]* '"' ;
IDENT : [a-zA-Z_] [a-zA-Z0-9_]* ;
HEX_INTEGER_CONSTANT : [-+]? '0' [xX][0-9a-fA-F]+ ;
INTEGER_CONSTANT : [-+]? [0-9]+;
FLOAT_CONSTANT : '-'? [0-9]+ '.' [0-9]+ (('e'|'E') ('+'|'-')? [0-9]+ )? ;

BLOCK_COMMENT:	'/*' .*? '*/' -> channel(HIDDEN);
COMMENT : '//' ~[\r\n]* -> channel(HIDDEN);
WHITESPACE : [ \t\r\n] -> skip ;