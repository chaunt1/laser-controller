/**************************************************************************************
 This software is developed by NEWSON NV
 Redistribution and use, with or without modification, are permitted
 
 THIS SOFTWARE IS PROVIDED BY NEWSON AS IS.
 ANY EXPRESS OR IMPLIED WARRANTIES ARE DISCLAIMED.
 IN NO EVENT SHALL NEWSON BE LIABLE FOR ANY DAMAGES ARISING
 OUT OF THE USE OF THIS SOFTWARE.
 **************************************************************************************/

#ifdef __cplusplus
extern "C" {
#endif

#ifdef _BUILD_DLL_
  #define RHOTHOR __declspec(dllexport)
#else
  #define RHOTHOR __declspec(dllimport)
#endif

// error codes
#define ERR_OK -1                    /* ok                                   */
#define ERR_BUSY 2                   /* system is processing job             */
#define ERR_JOB 3                    /* error processing job/empty job       */
#define ERR_HARDWARE 5               /* hardware not responding              */
#define ERR_DATA 13                  /* can't process data                   */
#define ERR_IMPLEMENTATION 23        /* function not supported               */
#define ERR_FLASH 45                 /* flash memory not enabled             */
#define ERR_INTERLOCK 48             /* IO status aborts execution           */
#define WARNING_BIOS 49              /* incompatible bios                    */
#define WARNING_SYSTEM 256           /* system still ok but issues a warning */

// rtListOpen parameter Mode options
#define LIST_START 1
#define AUTO_START 2
#define BOOT_START 3
#define LOAD_START 4

/************************************************************/
/*** accessing functions                                  ***/
/************************************************************/

RHOTHOR long __stdcall rtGetFirstFreeUSBDevice(char* Name);
RHOTHOR long __stdcall rtGetID(char* Name);
RHOTHOR long __stdcall rtGetIP(char* Mac, char* IP);
RHOTHOR long __stdcall rtGetNextFreeUSBDevice(char* Name);
RHOTHOR long __stdcall rtGetQueryTarget(long* Index);
RHOTHOR long __stdcall rtGetSerial(long* Serial);
RHOTHOR long __stdcall rtGetTarget(long* Mask);
RHOTHOR long __stdcall rtGetVersion(char* Version);
RHOTHOR long __stdcall rtSelectDevice(const char* IP);
RHOTHOR long __stdcall rtSetQueryTarget(long Index);
RHOTHOR long __stdcall rtSetTarget(long Mask);
RHOTHOR long __stdcall rtSystemCheck(long *Target, long* Powered, long* Bios);

/************************************************************/
/*** control and status functions                         ***/
/************************************************************/

RHOTHOR long __stdcall rtAbort();
RHOTHOR long __stdcall rtGetStatus(long* Memory);
RHOTHOR long __stdcall rtFileOpen(const char* FileName);
RHOTHOR long __stdcall rtFileClose();
RHOTHOR long __stdcall rtFileCloseAtHost();
RHOTHOR long __stdcall rtFileCloseAtIndex(long Index);
RHOTHOR long __stdcall rtListClose();
RHOTHOR long __stdcall rtListOpen(long Mode);
RHOTHOR long __stdcall rtReset();
RHOTHOR long __stdcall rtSystemSelectFile(long Index);
RHOTHOR long __stdcall rtSystemSuspend();
RHOTHOR long __stdcall rtSystemTableStop();
RHOTHOR long __stdcall rtSystemResume();

/************************************************************/
/*** server functions                                     ***/
/************************************************************/

RHOTHOR double __stdcall rtNaN_IO(long nr);

/************************************************************/
/*** front end functions                                  ***/
/************************************************************/

// flash functions
RHOTHOR long __stdcall rtEraseFromFlash(const char* FileName);
RHOTHOR long __stdcall rtFileUpload(const char* SrcFile, const char* FileName);
RHOTHOR long __stdcall rtFileUploadAtIndex(const char* SrcFile, const char* FileName, long Index);
RHOTHOR long __stdcall rtFileDownload(const char* FileName, const char* DestFile);
RHOTHOR long __stdcall rtFormatFlash();
RHOTHOR long __stdcall rtGetFileIndex(const char* FileName, long* Index);
RHOTHOR long __stdcall rtGetFlashFirstFileEntry(char* Name, long* Size);
RHOTHOR long __stdcall rtGetFlashNextFileEntry(char* Name, long* Size);
RHOTHOR long __stdcall rtGetFlashMemorySizes(long* Total, long* Allocated);

// profile store and load
RHOTHOR long __stdcall rtLoadProfile(const char* FileName, long Include);
RHOTHOR long __stdcall rtStoreProfile(const char* FileName, long Include);
#define INCL_CONFIG       0x0001
#define INCL_TGTMAP       0x0002
#define INCL_CALIBRATION  0x0004
#define INCL_FILES        0x0008  
#define INCL_BIOSVERSION  0x0010
#define INCL_DEFLDATA     0x0020
#define FORMAT_JSON       0x0100

// system functions
RHOTHOR long __stdcall rtGetCanLink(long Address, long* Value);
RHOTHOR long __stdcall rtSystemUartOpen(long baudrate, char parity, char stopbits);
RHOTHOR long __stdcall rtSystemUartWrite(long bytes, char* data);
RHOTHOR long __stdcall rtSystemUDPsend(char* IP, short port, char* data);
RHOTHOR long __stdcall rtUartRead(long* bytes, char* data);

// rtListOpen(1,2,3 or 4) or rtFileOpen()
RHOTHOR long __stdcall rtAcceptData(long DataType);
RHOTHOR long __stdcall rtOpenCanLink(long Baudrate);
RHOTHOR long __stdcall rtScanCanLink(long Address, long Node, long Index, long SubIndex);
RHOTHOR long __stdcall rtSetCanLink(long Node, long Index, long SubIndex, const char* Data);
RHOTHOR long __stdcall rtUDPsend(char* IP, short port, char* data);
RHOTHOR long __stdcall rtWaitCanLink(long Address, long Value, long Mask);
RHOTHOR long __stdcall rtMessage(char* info);

/************************************************************/
/*** single target functions  (rtSetTarget-lowest bit)    ***/
/*** Those functions query or control only the least      ***/
/*** significant selected target                          ***/
/*** rtSetTaget(0b*******1)-> Target 1                    ***/ 
/*** rtSetTaget(0b******10)-> Target 2                    ***/ 
/*** rtSetTaget(0b10000000)-> Target 8                    ***/ 
/************************************************************/

// calibration functions
RHOTHOR long __stdcall rtAddCalibrationData(const char* FileName);
RHOTHOR long __stdcall rtLoadCalibrationFile(const char* FileName);
RHOTHOR long __stdcall rtResetCalibration();
RHOTHOR long __stdcall rtSetResolverCal(const char* FileName);
RHOTHOR long __stdcall rtStoreCalibrationFile(const char* FileName);

// rtListOpen(1,2,3 or 4) or rtFileOpen()
RHOTHOR long __stdcall rtWaitIdle();
RHOTHOR long __stdcall rtWaitIO(long Value, long Mask);
RHOTHOR long __stdcall rtWaitResolver(long Nr, double TriggerPos, long TriggerMode);
RHOTHOR long __stdcall rtWaitStall();

// rtListOpen(3 or 4) or rtFileOpen()
RHOTHOR long __stdcall rtElseIfIO(long Value, long Mask);
RHOTHOR long __stdcall rtIfIO(long Value, long Mask);
RHOTHOR long __stdcall rtWhileIO(long Value, long Mask);

/************************************************************/
/*** broadcast target functions (rtSetTarget)             ***/
/*** Those functions control all selected targets         ***/
/*** rtSetTarget(0b01000001)-> Target 1 and 7             ***/
/************************************************************/

// rtListOpen(1,2,3 or 4) or rtFileOpen()
RHOTHOR long __stdcall rtArcTo(double X, double Y, double BF);
RHOTHOR long __stdcall rtArcMoveTo(double X, double Y, double BF);
RHOTHOR long __stdcall rtBurst(long Time);
RHOTHOR long __stdcall rtBurstEx(long Time1, long Time2, double PulseWidth2);
RHOTHOR long __stdcall rtCalcCorr(long Mode);
RHOTHOR long __stdcall rtCircle(double X, double Y, double Angle);
RHOTHOR long __stdcall rtCircleMove(double X, double Y, double Angle);
RHOTHOR long __stdcall rtDecrementCounter();
RHOTHOR long __stdcall rtDecrementCounter2();
RHOTHOR long __stdcall rtFileFetch(const char* FileName);
RHOTHOR long __stdcall rtIncrementCounter();
RHOTHOR long __stdcall rtIncrementCounter2();
RHOTHOR long __stdcall rtIndexFetch(long Index);
RHOTHOR long __stdcall rtJumpTo(double X, double Y);
RHOTHOR long __stdcall rtJumpTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtLineTo(double X, double Y);
RHOTHOR long __stdcall rtLineTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtLineToXD(double X, double Y, double Z, double TX, double TY, double TZ, long Mask);
RHOTHOR long __stdcall rtMoveTo(double X, double Y);
RHOTHOR long __stdcall rtMoveTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtMoveToXD(double X, double Y, double Z, double TX, double TY, double TZ, long Mask);
RHOTHOR long __stdcall rtPowerProfileTo(double X, double Y, char* pixels);
RHOTHOR long __stdcall rtPrint(char* data);
RHOTHOR long __stdcall rtPulse(double X, double Y);
RHOTHOR long __stdcall rtResetCounter();
RHOTHOR long __stdcall rtResetCounter2();
RHOTHOR long __stdcall rtResetEventCounter();
RHOTHOR long __stdcall rtResetResolver(long Nr);
RHOTHOR long __stdcall rtSetAnalog(long Value, long Mask);
RHOTHOR long __stdcall rtSetCfgIO(long Nr, long Value);
RHOTHOR long __stdcall rtChainHdr(double GridSize, long Period, long ColorMode, long rgb);
RHOTHOR long __stdcall rtChainStep(long GX, long GY, long red, long green, long blue, long Gate);
RHOTHOR long __stdcall rtChainTune(long SP, long SR, long SG, long SB, long rgb);
RHOTHOR long __stdcall rtSetCounter(long Value);
RHOTHOR long __stdcall rtSetCounter2(long Value);
RHOTHOR long __stdcall rtSetCorr(double a10, double a11, double a12, double a13, double a20, double a21, double a22, double a23);
RHOTHOR long __stdcall rtSetDefl(double a10, double a11, double a12, double a20, double a21, double a22);
RHOTHOR long __stdcall rtSetFieldSize(double Size);
RHOTHOR long __stdcall rtSetFieldSizeZ(double Size);
RHOTHOR long __stdcall rtSetFiducial(double X, double Y, double dX, double dY);
RHOTHOR long __stdcall rtSetFont(const char* FileName);
RHOTHOR long __stdcall rtSetImageMatrix(double a11, double a12, double a21, double a22);
RHOTHOR long __stdcall rtSetImageOffsXY(double X, double Y);
RHOTHOR long __stdcall rtSetImageOffsRelXY(double X, double Y);
RHOTHOR long __stdcall rtSetImageOffsZ(double Z);
RHOTHOR long __stdcall rtSetImageRotation(double Angle);
RHOTHOR long __stdcall rtSetIO(long Value, long Mask);
RHOTHOR long __stdcall rtSetLaser(bool OnOff);
RHOTHOR long __stdcall rtSetLaserFirstPulse(double Time);
RHOTHOR long __stdcall rtSetLaserLink(long Address, long Value);
RHOTHOR long __stdcall rtSetLaserTimes(long GateOnDelay, long GateOffDelay);
RHOTHOR long __stdcall rtSetMatrix(double a11, double a12, double a21, double a22);
RHOTHOR long __stdcall rtSetMinGatePeriod(long Time);
RHOTHOR long __stdcall rtSetOTF(long Nr, bool On);
RHOTHOR long __stdcall rtSetOffsIndex(long Index);
RHOTHOR long __stdcall rtSetOffsXY(double X, double Y);
RHOTHOR long __stdcall rtSetOffsZ(double Z);
RHOTHOR long __stdcall rtSetOscillator(long Nr, double Period, double PulseWidth);
RHOTHOR long __stdcall rtSetPower(long pwr);
RHOTHOR long __stdcall rtSetPowerLevels(long pwr100, long pwr0);
RHOTHOR long __stdcall rtSetPowerProfile(char* pixels);
RHOTHOR long __stdcall rtSetPulseBulge(double Factor);
RHOTHOR long __stdcall rtSetResolver(long Nr, double StepSize, double Range);
RHOTHOR long __stdcall rtSetResolverPosition(long Nr, double Position);
RHOTHOR long __stdcall rtSetResolverPositionRel(long Nr, double Position);
RHOTHOR long __stdcall rtSetResolverTrigger(long Nr, double Position, long IO);
RHOTHOR long __stdcall rtSetRotation(double Angle);
RHOTHOR long __stdcall rtSetScale(double Scale);
RHOTHOR long __stdcall rtSetSpeed(double Speed);
RHOTHOR long __stdcall rtSetTable(long Nr, double Position);
RHOTHOR long __stdcall rtSetTableDelay(long Nr, long Delay);
RHOTHOR long __stdcall rtSetTableLimitSwitches(long Nr, long MinStop, long MaxStop);
RHOTHOR long __stdcall rtSetTableMaxSpeed(long Nr, double Speed);
RHOTHOR long __stdcall rtSetTableRange(long Nr, long Type, double Value);
RHOTHOR long __stdcall rtSetTableSnapSize(long Nr, double SnapSize);
RHOTHOR long __stdcall rtSetTableSnapSizeEx(long Nr, double SnapSize, double ScanRange);
RHOTHOR long __stdcall rtSetTableSpeed(double Speed);
RHOTHOR long __stdcall rtSetTableStepSize(long Nr, double StepSize);
RHOTHOR long __stdcall rtSetVarBlock(long i, char data);
RHOTHOR long __stdcall rtSetWhileIO(long Value, long Mask);
RHOTHOR long __stdcall rtSetWobble(double Diam, long Freq);
RHOTHOR long __stdcall rtSetWobbleEx(long Type, double B, long b, long dy, double A, long a, long dx);
RHOTHOR long __stdcall rtSetWobbleEx(long nType, double nAmpl, long nFreq, long tType, double tAmpl, long tHarm, long tPhase);
RHOTHOR long __stdcall rtSetWobbleMode(long Dir, long Phase);
RHOTHOR long __stdcall rtSetJumpSpeed(double Speed);
RHOTHOR long __stdcall rtSleep(long Time);
RHOTHOR long __stdcall rtSynchronise();
RHOTHOR long __stdcall rtSystemChainTune(long SP, long SR, long SG, long SB, long rgb);
RHOTHOR long __stdcall rtSystemSetScale(double Scale);
RHOTHOR long __stdcall rtSystemSetRotation(double Angle);
RHOTHOR long __stdcall rtTableArcTo(double X, double Y, double BF);
RHOTHOR long __stdcall rtTableJog(long Nr, double Speed, long WhileIO);
RHOTHOR long __stdcall rtTableJumpTo3D(double X, double Y, double Z, double SpeedX, double SpeedY, double SpeedZ);
RHOTHOR long __stdcall rtTableMove(long Nr, double Target);
RHOTHOR long __stdcall rtTableLineTo(double X, double Y);
RHOTHOR long __stdcall rtTableLineTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtTableMoveTo(double X, double Y);
RHOTHOR long __stdcall rtTableMoveTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtWaitPosition(double Window);
// RHOTHOR long __stdcall rtTableSpin(long Nr, double Speed);
RHOTHOR long __stdcall rtPulse3D(double X, double Y, double Z);

// rtListOpen(3 or 4) or rtFileOpen()
RHOTHOR long __stdcall rtCharDef(long Ascii);
RHOTHOR long __stdcall rtDoLoop();
RHOTHOR long __stdcall rtDoWhile();
RHOTHOR long __stdcall rtElse();
RHOTHOR long __stdcall rtEndIf();
RHOTHOR long __stdcall rtFontDef(const char* Name);
RHOTHOR long __stdcall rtFontDefEnd();
RHOTHOR long __stdcall rtSetLoop(long LoopCtr);
RHOTHOR long __stdcall rtSuspend();
RHOTHOR long __stdcall rtVarBlockFetch(long Start, long Size, const char* FontName);

// rtListOpen(3)
RHOTHOR long __stdcall rtSetAutomation();
RHOTHOR long __stdcall rtDoAutomation();


/************************************************************/
/*** query target functions (rtSetQueryIndex)             ***/
/*** Those functions query or control the target          ***/
/*** selected by rtSetQueryIndex(1 to 8)                  ***/ 
/************************************************************/

RHOTHOR long __stdcall rtGetAnalog(long Nr, long* Value);
RHOTHOR long __stdcall rtGetCfgIO(long Nr, long *Value);
RHOTHOR long __stdcall rtGetCounter(long* Value);
RHOTHOR long __stdcall rtGetCounter2(long* Value);
RHOTHOR long __stdcall rtGetDeflReplies(long* CH1, long* CH2, long* CH3);
RHOTHOR long __stdcall rtGetFieldSize(double* Size);
RHOTHOR long __stdcall rtGetFieldSizeZ(double* Size);
RHOTHOR long __stdcall rtGetIO(long* Value);
RHOTHOR long __stdcall rtGetLaserLink(long Address, long* Value);
RHOTHOR long __stdcall rtGetMaxSpeed(double* Speed);
RHOTHOR long __stdcall rtGetResolvers(double* X, double* Y);
RHOTHOR long __stdcall rtGetScannerDelay(long* Delay);
RHOTHOR long __stdcall rtGetSetpointFilter(long* TimeConst);
RHOTHOR long __stdcall rtGetTablePositions(double* X, double* Y, double* Z);
RHOTHOR long __stdcall rtQueryDefl(long channel, long command, long* reply);

RHOTHOR long __stdcall rtSystemSetAnalog(long Value, long Mask);
RHOTHOR long __stdcall rtSystemSetOffsXY(double X, double Y);
RHOTHOR long __stdcall rtSystemSetIO(long Value, long Mask);
RHOTHOR long __stdcall rtSystemSetResolverSpeed(long Nr, double Speed);
RHOTHOR long __stdcall rtSystemSetWhileIO(long Value, long Mask);
RHOTHOR long __stdcall rtSystemTableMove(long Nr, double Position);
RHOTHOR long __stdcall rtSystemTableMoveRel(long Nr, double Offset);

/************************************************************/
/*** reserved rhothor functions                           ***/
/************************************************************/

#define SET_USB_VID_PID 0x10000
#define GET_USB_VID_PID 0x10001

RHOTHOR long __stdcall rtParse(const char* Cmd);
RHOTHOR long __stdcall rtRunServer(long Id, void* Params1, void* Params2);

/************************************************************/
/*** not supported functions, kept for compatibility      ***/
/************************************************************/

RHOTHOR long __stdcall rtSetLead(long Time);
RHOTHOR long __stdcall rtSetMaxSpeed(double Speed);
RHOTHOR long __stdcall rtSetHover(long Time);
RHOTHOR long __stdcall rtSetImageMatrix3D(double a11, double a12, double a21, double a22, double a31, double a32);
RHOTHOR long __stdcall rtSetResolverRange(long Nr, double Range);
RHOTHOR long __stdcall rtSetTableOffsXY(double X, double Y);
RHOTHOR long __stdcall rtSetTableSnap(double Distance);
RHOTHOR long __stdcall rtSetTableWhileIO(long Value, long Mask);
RHOTHOR long __stdcall rtSendUartLink(const char* Data);
RHOTHOR long __stdcall bcSamplePoint(double X, double Y, long Row, long Col, double Sweep, double* OffsetX, double* OffsetY);
RHOTHOR long __stdcall bcSelectDevice(const char* CommPort);
RHOTHOR long __stdcall rtLoadCalibration();
RHOTHOR long __stdcall rtStoreCalibration();
RHOTHOR long __stdcall rtAddCalibrationDataZ(const char* FileName);
RHOTHOR long __stdcall rtResetCalibrationZ();
RHOTHOR long __stdcall rtStoreCalibrationFileZ(const char* FileName);
RHOTHOR long __stdcall rtLoadCalibrationFileZ(const char* FileName);
RHOTHOR long __stdcall rtWaitEventCounter(long Count);

#ifdef __cplusplus
}
#endif
