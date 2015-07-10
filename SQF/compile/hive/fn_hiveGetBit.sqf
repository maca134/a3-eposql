private ['_table', '_id', '_bit', '_packet', '_return', '_result', '_response', '_status', '_data'];
_table = [_this, 0, '', ['']] call BIS_fnc_param;
_id = [_this, 1, '', ['',0]] call BIS_fnc_param;
_bit = [_this, 2, 0, [0]] call BIS_fnc_param;

_packet = ('GETBIT' + EPOSQL_SEP + _table + EPOSQL_SEP + _id + EPOSQL_SEP + str (_bit) + EPOSQL_SEP);

_return = false;
try {
	_result = "EpoSql" callExtension _packet;
	_response = call compile _result;
	if (isNil '_response') then {throw '_response is nil';};
	if (typeName _response != 'ARRAY') exitWith {throw '_response isnt an array';};
	if !((count _response) in [2,3]) then {throw 'invalid response';};
	_status = _response select 0;
	if (_status == 0) then {throw 'status is 0';};
	_return = (_response select 1) == '1';
} catch {
	_return = false;
	(format ['[GETBIT ERROR]: %1', _exception]) call ESQ_fnc_log;
};
_return