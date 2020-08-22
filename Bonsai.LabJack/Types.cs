using LabJack;

namespace Bonsai.LabJack
{
    public enum DeviceType
    {
        Any = LJM.CONSTANTS.ctANY,
        T4 = LJM.CONSTANTS.dtT4,
        T7 = LJM.CONSTANTS.dtT7,
        TSeries = LJM.CONSTANTS.dtTSERIES,
        Digit = LJM.CONSTANTS.dtDIGIT
    }

    public enum ConnectionType
    {
        Any = LJM.CONSTANTS.ctANY,
        Usb = LJM.CONSTANTS.ctUSB,
        NetworkTcp = LJM.CONSTANTS.ctNETWORK_TCP,
        EthernetTcp = LJM.CONSTANTS.ctETHERNET_TCP,
        WifiTcp = LJM.CONSTANTS.ctWIFI_TCP,
        NetworkUdp = LJM.CONSTANTS.ctNETWORK_UDP,
        EthernetUdp = LJM.CONSTANTS.ctETHERNET_UDP,
        WifiUdp = LJM.CONSTANTS.ctWIFI_UDP,
        NetworkAny = LJM.CONSTANTS.ctNETWORK_ANY,
        EthernetAny = LJM.CONSTANTS.ctETHERNET_ANY,
        WifiAny = LJM.CONSTANTS.ctWIFI_ANY,
        AnyUdp = LJM.CONSTANTS.ctANY_UDP
    }
}
