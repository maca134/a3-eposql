private ['_table', '_id', '_packet', '_return', '_result', '_response', '_status', '_data', '_ttl', '_tid', '_parts', '_p', '_responseParts'];
_table = [_this, 0, '', ['']] call BIS_fnc_param;
_id = [_this, 1, '', ['',0]] call BIS_fnc_param;

_packet = ('GETSYNC' + EPOSQL_SEP + _table + EPOSQL_SEP + _id + EPOSQL_SEP);

_return = [0,[]];
try {
	_result = "EpoSql" callExtension _packet;
	_response = call compile _result;
	if (isNil '_response') then {throw '_response is nil';};
	if (typeName _response != 'ARRAY') exitWith {throw '_response isnt an array';};
	if !((count _response) in [2,3]) then {throw 'invalid response';};

	_status = _response select 0;
	_data = '[]';
	switch (_status) do {
		case 0: {
			throw format['error: %1', _response select 1];
		};
		case 1: {
			_data = _response select 1;
			if (count _response == 3) then {
				_ttl = _response select 2;
			};
		};
		case 2: {
			_tid = _response select 1;
			_parts = _response select 2;
			_p = 0;
			_responseParts = '';
			while {true} do {
				_packet = ('RESPONSE' + EPOSQL_SEP + str(_tid) + EPOSQL_SEP + str(_p) + EPOSQL_SEP);
				_result = "EpoSql" callExtension _packet;
				_responseParts = _responseParts + _result;
				if ((_p + 1) == _parts) exitWith {};
				_p = _p + 1;
			};
			_response = nil;
			_response = call compile _responseParts;
			if (isNil '_response') then {throw 'Could not get response';};
			if (typeName _response != 'ARRAY') exitWith {throw '_response isnt an array';};
			if ((count _response) != 2) then {throw 'response invalid';};
			_data = _response select 1;
			if (count _response == 3) then {
				_ttl = _response select 2;
			};
		};
	};
	if (_data == '[]') then {
		throw 'Response is empty';
	};
	_response = nil;
	_response = call compile _data;
	if (isNil '_response') then {throw 'Could not get response';};
	if (typeName _response != 'ARRAY') exitWith {throw '_response isnt an array';};
	_return = [1, _response];
	if (!isNil '_ttl') then {
		_return pushBack _ttl;
	};
} catch {
	_return = [1,[]];
	(format ['[GET ERROR]: %1', _exception]) call ESQ_fnc_log;
};
_return