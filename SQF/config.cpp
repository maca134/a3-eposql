class CfgPatches {
	class A3_eposql_server {
		units[] = {};
		weapons[] = {};
		requiredVersion = 0.1;
		requiredAddons[] = {"A3_epoch_server"};
	};
};

class CfgFunctions {
	class ESQ {
		class Init {
			file = "\x\addons\a3_eposql_server\init";
			class init {
				preInit = 1;
			};
		};

		class Util {
			file = "\x\addons\a3_eposql_server\compile\util";
			class log {};
		};

		class Server {
			file = "\x\addons\a3_eposql_server\compile\hive";
			class hiveSet {};
			class hiveGet {};
			class hiveGetBit {};
			class hiveExpire {};
			class hiveDel {};
			class hiveLog {};
		};
	};
};