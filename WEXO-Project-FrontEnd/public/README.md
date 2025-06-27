# WEXO – Automatisk Lysstyring via Wi-Fi

WEXO-projektet er et automatisk lysstyringssystem, der bruger WEXOs Wi-Fi-netværk til at registrere tilstedeværelse. Når ingen godkendte enheder er forbundet til netværket, slukker lyset automatisk.  
Systemet tilbyder en brugervenlig grænseflade med funktioner som:

- Opsætning af aktive tidsrum
- Blacklisting af enheder
- Manuel aktivering/deaktivering af systemet
- Login-system til adgangskontrol

---

## 🛠️ Teknologier og værktøjer

- **Backend:** C# (.NET)
- **Frontend:** Vue.js
- **Database:** Microsoft SQL Server (MSSQL)
- **Sprog:** JavaScript, C#
- **IDE:** Visual Studio & Visual Studio Code

---

## 🚀 Installation og opsætning

Følgende guide hjælper dig med at sætte projektet op lokalt. Du skal bruge:
- Visual Studio (til backend)
- Node.js + npm (til frontend)
- SQL Server (til databasen)

### 1. Start backend

1. Åbn backend-projektet i **Visual Studio**
2. Sørg for, at forbindelsesstrengen til MSSQL er korrekt sat i `appsettings.json`
   ```
3. Kør projektet. 
### 2. Konfigurer frontendens baseURL

Gå til `frontend/src/components/icons/baseURLconfig.js` og sæt backendens URL korrekt:

> Hvis systemet skal hostes, kan dette ændres til et domæne.

### 3. Kør frontend

Åbn terminalen (eller kommandoprompt), og navigér til frontend-mappen:

```bash
cd [sti-til-frontend]
npm install
npm run dev
```

Frontend-serveren starter typisk på `http://localhost:5173`

### 4. Log ind og konfigurer systemet

1. Åbn webinterfacet i din browser
2. Log ind med en gyldig bruger (loginoplysninger udleveres separat)
3. Konfigurer systemet efter dine behov:
   - Vælg aktive tidsrum for lysstyring
   - Tilføj enheder til blacklist
   - Aktivér/deaktivér systemet

---

## 📋 Krav

- .NET SDK (til backend)
- Node.js og npm (til frontend)
- Microsoft SQL Server
- Adgang til WEXO Wi-Fi (eller simuleret netværkstilstand)

---

## 🔐 Login

Systemet benytter login for at give adgang til konfigurationssiden. Loginoplysninger er givet separet 


---

## 📄 Licens

Dette projekt er udviklet som en del af et internt eller uddannelsesmæssigt projekt.  

---
