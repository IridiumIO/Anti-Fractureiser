# Anti-Fractureiser

Just in case you don't want to (or don't know how to) manually run the scripts to detect infection from here: https://prismlauncher.org/news/cf-compromised-alert/#manual-check

Makes use heavily of shared scripts and data from here: https://github.com/fractureiser-investigation/fractureiser

![Screenshot 2023-06-08 045928](https://github.com/IridiumIO/Anti-Fractureiser/assets/1491536/3f7abe14-97cd-4a2f-b642-55a6b684e009)


---


## Usage
1. Download and run the exe file as an Administrator. 
2. Click "Run Check" to check for existing malware payloads on your system. 
3. Click "Create Defender Rules" to block access from the known malicious IP addresses and URLs as listed below
4. Head over [here](https://github.com/MCRcortex/nekodetector) and run the detector to find any compromised .jar files on your system. At present you will have to compile this yourself but I'm sure the dev will release an executable progam soon. 

#### Malicious Entries that are searched for:

- `%LocalAppdata%\Microsoft Edge`
    - `\.ref`
    - `\.client.jar` (the big one)
    - `\.lib.dll`
    - `\.libWebGL64.jar`
    - `\run.bat`
- `%AppData%\Microsoft\Windows\Start Menu\Programs\Startup\run.bat`
- Registry `HKCU\Software\Microsoft\Windows\CurrentVersion\Run\t`

#### Malicious IP Addresses blocked in Windows Firewall:
- `85.217.144.130`
- `107.189.3.101`
- `95.214.27.172`
- `171.22.30.117`


#### Malicious URLs blocked in Hosts
- files-8ie.pages.dev
- connect.skyrage.de
- t23e7v6uz8idz87ehugwq.skyrage.de
- files.skyrage.de
- file.skyrage.de
- qw3e1ee12e9hzheu9h1912hew1sh12uw9.skyrage.de
