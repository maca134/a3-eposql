private ['_table', '_id', '_exp', '_data', '_packet'];

_table = [_this, 0, '', ['']] call BIS_fnc_param;
_id = [_this, 1, '', ['',0]] call BIS_fnc_param;
_exp = [_this, 2, '-1', ['',0]] call BIS_fnc_param;
_data = str(_this select 3);
if (typeName _exp == "SCALAR") then {_exp = str _exp;};

_packet = ('SET' + EPOSQL_SEP + _table + EPOSQL_SEP + _id + EPOSQL_SEP + _exp + EPOSQL_SEP + _data + EPOSQL_SEP);
(format ['[SET]: %1', _packet]) call ESQ_fnc_log;
EPOSQL_DLL callExtension _packet;