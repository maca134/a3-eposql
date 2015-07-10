using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace EpoSqlConsole
{
    class Program
    {
        private static ARMAExt _ext = new ARMAExt();

        private static List<string> _tests = new List<string>() { 
            
            /*"SET\nVehicle\nOP1EUAltis:10\n345600\n[\"O_Heli_Transport_04_bench_EPOCH\",[[16746.2,11390.5,0.218281],[-0.914493,-0.36192,0.180876],[0.0861715,0.262568,0.961058]],0.0165969,[0,0,0,0,0,0,0,0,0.0341024,0.0165962,0.0555296,0.0208126,0.035347,0.0161221,0.0200722,0.0152663,0.0419891,0.0169723,0.0195919,0.0137141,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0257383,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],1,[[],[[\"JackKit\"],[1]],[[],[]],[[\"H_74_EPOCH\",\"EpochRadio0\",\"ItemWatch\"],[1,1,1]]],[],0]\n",
            "SET\nVehicle\nOP1EUAltis:30\n345600\n[]\n",

            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111244\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111244\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111244\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111244\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayer\n76561198004111244\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",

            "SET\nVehicle\nOP1EUAltis:43\n345600\n[\"C_Quadbike_01_EPOCH\",[[18943.1,16700.7,0],[-0.191985,-0.981398,0],[0,0,1]],0,[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],1,[[],[[\"ItemDocument\",\"CinderBlocks\",\"JackKit\"],[1,1,1]],[[],[]],[[\"ItemWatch\"],[1]]],[],3]\n",
            "SET\nVehicle\nOP1EUAltis:30\n345600\n[\"O_Heli_Transport_04_bench_EPOCH\",[[18043.2,12321.5,2.56387],[0.800528,-0.5989,-0.0217454],[-0.504494,-0.653865,-0.563867]],1,[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],0.217448,[[],[[\"11Rnd_45ACP_Mag\",\"JackKit\"],[11,1]],[[],[]],[[],[]]],[],0]\n",
            "SET\nAI_ITEMS\nOP1EUAltis:22\n604800\n[[\"ItemSodaBurst\",\"meatballs_epoch\",\"MortarBucket\",\"CinderBlocks\",\"VehicleRepair\",\"CircuitParts\",\"ItemCorrugated\",\"PartPlankPack\",\"ItemRock\",\"ItemRope\",\"ItemStick\"],[5,5,5,5,5,5,5,5,5,5,5]]\n",
            "SET\nAI\nOP1EUAltis:9\n-1\n[\"C_man_1\",[11037,19395.9,0.330208],[[10996.2,19064.7,0.515503],[6,14]]]\n",
            "SET\nI\nOP1EUAltis\n86400\n[\"CONTINUE\"]\n",
            "SET\nBANK\n76561198004111275\n86400\n[1000]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[175.347,[15018.5,17813,0.00155258],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"\",\"V_41_EPOCH\",\"\",\"U_Test1_uniform\",\"Epoch_Male_F\"],[],[98.6,5000,2500,63,2500,0,0,150,0,0,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"\",[],[\"\",\"\",\"\"]],[\"ItemMap\"],[],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nStorage\nOP1EUAltis:1\n345600\n[\"Tipi_EPOCH\",[14.7024,[15032.3,17787.3,0.023632]],0,[[],[[],[]],[[],[]],[[],[]]],0,[],-1]\n",
            "SET\nBuilding\nOP1EUAltis:0\n345600\n[\"PlotPole_EPOCH\",[[15032.4,17784.4,0.627512],[-0.0835571,-0.0989595,0.991577],[-0.938477,-0.326781,-0.111695]],\"-1\",\"76561198004111275\",0,0]\n",
            "SET\nBuilding\nOP1EUAltis:0\n345600\n[\"PlotPole_EPOCH\",[[15032.4,17784.4,0.627512],[-0.0835571,-0.0989595,0.991577],[-0.938477,-0.326781,-0.111695]],\"-1\",\"76561198004111275\",0]\n",
            "SET\nBuilding\nOP1EUAltis:0\n345600\n[\"PlotPole_EPOCH\",[[15032.4,17784.4,0.627512],[-0.0835571,-0.0989595,0.991577],[-0.938477,-0.326781,-0.111695]]]\n",
            "SET\nVehicleLock\nOP1EUAltis:30\n345600\n[\"76561198004111275\"]\n",
            "SET\nPlayerData\n76561198004111275\n345600\n[\"maca134\"]\n",
            "SET\nPlayerStats\n76561198004111275\n-1\n[0,1]\n",
            "SET\nPLAYERS\nOP1EUAltis\n-1\n[\"76561198004111275\"]\n",
            "SET\nPLAYERS\nOP1EUAltis\n-1\n[]\n",
            "SET\nLOG\n-1\n-1\n[\"Test Type\",\"Test Message 1\"]\n",
            "SET\nLOG\n-1\n-1\n[\"Test Type\",\"Test Message 2\"]\n",
            "SET\nLOG\n-1\n-1\n[\"Test Type\",\"Test Message 3\"]\n",
            "SET\nLOG\n-1\n-1\n[\"Test Type\",\"Test Message 4\"]\n",
            "SET\nVehicle\nOP1EUAltis:43\n345600\n[]\n",
            "SET\nVehicle\nOP1EUAltis:77\n345600\n[\"O_Heli_Transport_04_bench_EPOCH\",[[16746.2,11390.5,0.218281],[-0.914493,-0.36192,0.180876],[0.0861715,0.262568,0.961058]],0.0165969,[0,0,0,0,0,0,0,0,0.0341024,0.0165962,0.0555296,0.0208126,0.035347,0.0161221,0.0200722,0.0152663,0.0419891,0.0169723,0.0195919,0.0137141,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0843919,0.0257383,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],1,[[],[[\"JackKit\"],[1]],[[],[]],[[\"H_74_EPOCH\",\"EpochRadio0\",\"ItemWatch\"],[1,1,1]]],[],0]\n",
            "SET\nPlayer\n76561197982800832\n2592000\n[[95.4107,[11362.4,7937.61,20.6591],\"OP7EUPodagorsk\"],[0,0,1,0],[\"\",\"H_33_EPOCH\",\"V_39_EPOCH\",\"B_Carryall_cbr\",\"U_O_FullGhillie_ard\",\"Epoch_Male_F\"],[],[98.6,4073,1968,6701,195,0,0,0,0,189.74,0,[0,0,0,0],100,[0,0,1,0,0,0,0]],[\"MMG_01_tan_F\",[[\"MMG_01_tan_F\",\"muzzle_snds_93mmg_tan\",\"\",\"optic_AMS_khk\",[\"150Rnd_93x64_Mag\",78],\"bipod_02_F_tan\"],[\"hgun_ACPC2_F\",\"\",\"\",\"\",[\"9Rnd_45ACP_Mag\",9],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"],[\"MultiGun\",\"\",\"\",\"\",\"\"]],[\"MMG_01_tan_F\",\"\",\"hgun_ACPC2_F\"]],[\"ItemMap\",\"ItemGPS\",\"NVG_EPOCH\",\"Rangefinder\"],[[\"EnergyPackLg\",4],[\"FoodWalkNSons\",1],[\"meatballs_epoch\",1],[\"meatballs_epoch\",1],[\"FoodWalkNSons\",1],[\"ItemCoolerE\",1],[\"7Rnd_408_Mag\",5],[\"7Rnd_408_Mag\",5],[\"150Rnd_93x64_Mag\",150],[\"150Rnd_93x64_Mag\",150],[\"VehicleRepair\",1],[\"VehicleRepair\",1],[\"VehicleRepair\",1]],[[[\"Repair_EPOCH\"],[1]],[[\"optic_Hamr\",\"Heal_EPOCH\"],[1,1]],[[],[]]],[[[],[]],[[\"MultiGun\"],[1]],[[],[]]],\"76561198033181194\",true]\n",
            "SET\nPlayer\n76561197996850101\n2592000\n[[69.4259,[9904.97,9358.61,0.480211],\"OP7EUPodagorsk\"],[0,0,1,0],[\"\",\"H_41_EPOCH\",\"V_6_EPOCH\",\"B_Carryall_mcamo\",\"U_C_Poloshirt_stripped\",\"Epoch_Male_F\"],[],[98.6,4139,1596,8105,228.2,0,0,0,0,146.357,180,[0.0022516,0.0022516,0.00307036,0.00691615],100,[0,0,0,0,2,0,0]],[\"LMG_Mk200_F\",[[\"MultiGun\",\"Repair_EPOCH\",\"\",\"\",[\"EnergyPackLg\",90],\"\"],[\"LMG_Mk200_F\",\"muzzle_snds_H_MG\",\"acc_pointer_IR\",\"optic_MRCO\",[\"200Rnd_65x39_cased_Box\",170],\"bipod_02_F_hex\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"],[\"1911_pistol_epoch\",\"\",\"\",\"\",\"\"]],[\"LMG_Mk200_F\",\"\",\"MultiGun\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"EpochRadio8\",\"ItemGPS\",\"NVG_EPOCH\",\"Rangefinder\"],[[\"9rnd_45X88_magazine\",9],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"ColdPack\",1],[\"PaintCanGrn\",1],[\"PaintCanGrn\",1],[\"200Rnd_65x39_cased_Box\",81],[\"SatchelCharge_Remote_Mag\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[\"1911_pistol_epoch\"],[1]],[[],[]]],\"76561198172262223\",true]\n",
            "SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n",
            "SET\nPlayerStats\n76561198004111275\n-1\n[0,1]\n",
            "SET\nLockBoxPin\nOP1EUAltis:82\n345600\n[\"1994\"]\n",
            "SET\nLockBoxPin\nOP1EUAltis:83\n345600\n[\"2222\"]\n",
            "SET\nLockBoxPin\nOP1EUAltis:84\n345600\n[\"3333\"]\n",
            "SET\nLockBoxPin\nOP1EUAltis:85\n345600\n[\"4444\"]\n",
            "GETBIT\nPlayerStats\n76561197982800832\n0\n",
            "GETSYNC\nPlayer\n76561198004111244\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",
            "GETSYNC\nPlayer\n76561198004111275\n",*/
            //"GETBIT\nPlayerStats\n76561197982800832\n0\n",
            //"DEL\nVehicleLock\nOP1EUAltis:4\n"
            "SET\nVehicle\nOP1EUAltis:43\n345600\n[\"C_Van_01_box_EPOCH\",[[19771.9,15070.9,-9.53674e-007],[-0.813206,-0.581976,0],[0,0,1]],0,[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],1,[[],[[\"SmokeShellOrange\",\"30Rnd_65x39_caseless_mag\",\"jerrycan_epoch\",\"JackKit\"],[1,30,1,1]],[[],[]],[[],[]]],[],1]",
            
        };

        static void Main(string[] args)
        {
            _ext.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "EpoSql.dll"));
            Console.WriteLine("Loading DLL...");
            _ext.Invoke("LOAD");
            Thread.Sleep(2000);
            Console.WriteLine("Loaded DLL");
            Console.WriteLine("--------------");

            /*
            _ext.Invoke("SET\nPlayer\n76561198004111275\n2592000\n[[221.568,[18795.5,13024.6,0.00173187],\"OP1EUAltis\"],[0,0,1,0],[\"G_Bandanna_sport\",\"H_Bandanna_surfer_blk\",\"V_2_EPOCH\",\"B_Carryall_cbr\",\"U_C_WorkerCoveralls\",\"Epoch_Male_F\"],[],[98.6,5000,2500,83,2500,0,0,150,0,2500,0,[0,0,0,0],100,[0,0,0,0,0,0,0]],[\"hgun_P07_F\",[[\"hgun_P07_F\",\"\",\"\",\"\",[\"16Rnd_9x21_Mag\",16],\"\"],[\"Rangefinder\",\"\",\"\",\"\",\"\"]],[\"\",\"\",\"hgun_P07_F\"]],[\"ItemMap\",\"ItemCompass\",\"ItemWatch\",\"ItemGPS\",\"Rangefinder\"],[[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"ItemSodaOrangeSherbet\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"CookedSheep_EPOCH\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"FAK\",1],[\"ItemBikeKit\",1]],[[[],[]],[[],[]],[[],[]]],[[[],[]],[[],[]],[[],[]]],\"\",true]\n");
            Thread.Sleep(2000);

            Stopwatch t = Stopwatch.StartNew();
            for (var i = 0; i < 10000; i++)
            {
                _ext.Invoke("GETSYNC\nPlayer\n76561198004111275\n");
            }
            t.Stop();
            long e = t.ElapsedMilliseconds;
            Thread.Sleep(5000);
            Console.WriteLine(String.Format("Command took {0} ms to run. {1}", e, (float)e / 10000));
            Console.ReadKey();
            */

            foreach (string s in _tests)
            {
                Stopwatch watch = Stopwatch.StartNew();
                Console.WriteLine(_ext.Invoke(s));
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine(String.Format("Command took {0} ms to run", elapsedMs));
                Console.WriteLine("--------------");
                //Thread.Sleep(1000);
            }

            while (true)
            {
                Console.Write("Enter Command: ");
                string cmd = Console.ReadLine();
                if (cmd == "")
                {
                    _ext.Unload();
                    break;
                }

                Stopwatch watch = Stopwatch.StartNew();
                cmd = cmd + "\n";
                string result = _ext.Invoke(cmd);
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine(String.Format("Command took {0} ms to run", elapsedMs));
                Console.WriteLine(result);
                Console.WriteLine("--------------");
            }
        }
    }
}
