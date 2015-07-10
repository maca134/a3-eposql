private ['_type', '_entry', '_packet'];

_type = [_this, 0, '', ['']] call BIS_fnc_param;
_entry = [_this, 1, '', ['']] call BIS_fnc_param;

if (_type == '' || _entry == '') exitWith {};

_packet = ('SET' + EPOSQL_SEP + 'LOG' + EPOSQL_SEP + '-1' + EPOSQL_SEP + '-1' + EPOSQL_SEP + str([_type, _entry]) + EPOSQL_SEP);

EPOSQL_DLL callExtension _packet;