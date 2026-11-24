#ifdef __cplusplus
extern "C" {
#endif

#ifdef _BUILD_DLL_
  #define RHOTHOR __declspec(dllexport)
#else
  #define RHOTHOR __declspec(dllimport)
#endif


#define ERR_OK -1                    /* ok                                 */
#define ERR_BUSY 2                   /* system is processing job           */
#define ERR_JOB 3                    /* error processing job/empty job     */
#define ERR_HARDWARE 5               /* hardware not responding            */
#define ERR_MEMORY 15                /* can't allocate memory              */
#define ERR_DATA 13                  /* can't process data                 */
#define ERR_IMPLEMENTATION 23        /* function not supported             */
#define ERR_LASER_LOW 42             /* too little laser power             */
#define ERR_TRACKING 43              /* system is not running              */
#define ERR_LASER_HIGH 46            /* too much laser power               */

#define LIST_START 1
#define AUTO_START 2
#define AUTO_REPEAT 3


/*** accessing functions ***********************************************************/

RHOTHOR long __stdcall rtGetFreeDevList(long* SerialNrs, long* Size);
RHOTHOR long __stdcall rtInitialise();
RHOTHOR long __stdcall rtSelectDevice(long SerialNr);

/*** query functions ***************************************************************/

RHOTHOR long __stdcall rtGetChannelData(long nr, long* Setpoint, long* Actual, double* Ieff);
RHOTHOR long __stdcall rtGetFieldSize(double* Size);
RHOTHOR long __stdcall rtGetIO(long* Value);
RHOTHOR long __stdcall rtGetMaxSpeed(double* Speed);
RHOTHOR long __stdcall rtGetResolvers(double* X, double* Y);
RHOTHOR long __stdcall rtGetScannerDelay(long* Delay);
RHOTHOR long __stdcall rtGetSerial(long* Serial);
RHOTHOR long __stdcall rtGetSetpointFilter(long* TimeConst);
RHOTHOR long __stdcall rtGetStatus(long* Memory);
RHOTHOR long __stdcall rtGetVersion(char* Version, long MaxSize);

/*** align functions ***************************************************************/

RHOTHOR long __stdcall rtAddCalibrationData(const char* FileName);
RHOTHOR long __stdcall rtLoadCalibrationFile(const char* FileName);
RHOTHOR long __stdcall rtResetCalibration();
RHOTHOR long __stdcall rtSamplePoint(double X, double Y, 
                                     double* OffsetX, double* OffsetY);
RHOTHOR long __stdcall rtSamplePoint3D(double X, double Y, double Z, 
                                       double* OffsetX, double* OffsetY, double* OffsetZ);
RHOTHOR long __stdcall rtStoreCalibrationFile(const char* FileName);
RHOTHOR long __stdcall bcSamplePoint(double X, double Y,
                                     long Row, long Col, double Sweep,
                                     double* OffsetX, double* OffsetY);
RHOTHOR long __stdcall rtAddCalibrationDataZ(const char* FileName);
RHOTHOR long __stdcall rtLoadCalibrationFileZ(const char* FileName);
RHOTHOR long __stdcall rtResetCalibrationZ();
RHOTHOR long __stdcall rtStoreCalibrationFileZ(const char* FileName);

/*** control functions *************************************************************/

RHOTHOR long __stdcall rtAbort();
RHOTHOR long __stdcall rtArcTo(double X, double Y, double BF);
RHOTHOR long __stdcall rtBurst(long Time);
RHOTHOR long __stdcall rtFreqModOscillator(long Nr, double PulseWidth,
                                           double Speed1, double Freq1,
                                           double Speed2, double Freq2);
RHOTHOR long __stdcall rtJumpTo(double X, double Y);
RHOTHOR long __stdcall rtLineTo(double X, double Y);
RHOTHOR long __stdcall rtListClose();
RHOTHOR long __stdcall rtListOpen(long Mode);
RHOTHOR long __stdcall rtMoveTo(double X, double Y);
RHOTHOR long __stdcall rtPulse(double X, double Y);
RHOTHOR long __stdcall rtPulseModOscillator(long Nr, double Period,
                                            double Speed1, double PulseWidth1,
                                            double Speed2, double PulseWidth2);
RHOTHOR long __stdcall rtSetIO(long Value, long Mask);
RHOTHOR long __stdcall rtSetLaser(bool OnOff);
RHOTHOR long __stdcall rtSetLaserTimes(long GateOnDelay, long GateOffDelay);
RHOTHOR long __stdcall rtSetMatrix(double a11, double a12, double a21, double a22);
RHOTHOR long __stdcall rtSetMaxSpeed(double Speed);
RHOTHOR long __stdcall rtSetOffsXY(double X, double Y);
RHOTHOR long __stdcall rtSetOffsZ(double Z);
RHOTHOR long __stdcall rtSetOscillator(long Nr, double Period, double PulseWidth);
RHOTHOR long __stdcall rtSetResolver(long Nr, double StepSize, double Range);
RHOTHOR long __stdcall rtSetSpeed(double Speed);
RHOTHOR long __stdcall rtSetJumpSpeed(double Speed);
RHOTHOR long __stdcall rtSetUART(long Baudrate, long Bits, char Parity, long Stopbits);
RHOTHOR long __stdcall rtSetWindow(double Size);
RHOTHOR long __stdcall rtSleep(long Time);
RHOTHOR long __stdcall rtTransmitUART(char* OutString, long Size);
RHOTHOR long __stdcall rtWaitIO(long Value, long Mask);
RHOTHOR long __stdcall rtWaitResolver(long Nr, double TriggerPos, long TriggerMode);
RHOTHOR long __stdcall rtMoveTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtLineTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtJumpTo3D(double X, double Y, double Z);
RHOTHOR long __stdcall rtPulse3D(double X, double Y, double Z);

/*** control functions version *.*.1.* *********************************************/

RHOTHOR long __stdcall rtSetMinGatePeriod(long Time);
RHOTHOR long __stdcall rtSetPulseBulge(double Factor);

/*** flash functions ***************************************************************/

RHOTHOR long __stdcall rtFormatFlash();
RHOTHOR long __stdcall rtFileOpen(char* FileName);
RHOTHOR long __stdcall rtFileClose();
RHOTHOR long __stdcall rtEraseFromFlash(char* FileName);
RHOTHOR long __stdcall rtGetFlashFirstFileEntry(char* Name, long* Size);
RHOTHOR long __stdcall rtGetFlashNextFileEntry(char* Name, long* Size);
RHOTHOR long __stdcall rtGetFlashMemorySizes(long* Total, long* Allocated);
RHOTHOR long __stdcall rtFileFetch(char* FileName);

/*** reserved rhothor functions ****************************************************/

RHOTHOR long __stdcall rtRunServer(long Id, void* Params1, void* Params2);

#ifdef __cplusplus
}
#endif
