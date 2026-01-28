# 🤖 Chatterbox - NetOnNet AI Kundtjänst

En AI-driven chatbot byggd med ASP.NET Core MVC och Azure OpenAI för att hjälpa kunder med hemelektronik.

## 📋 Projektbeskrivning

Chatterbox är en intelligent chatbot som arbetar på NetOnNets kundtjänst. Han är 55 år gammal, bor i Stockholm och är expert inom hemelektronik. Chatboten använder Azure OpenAI för att ge hjälpsamma och professionella svar på frågor om TV, datorer, mobiler, ljudutrustning, hushållsapparater och annan hemelektronik.

## 🚀 Funktioner

✅ Säker input-hantering med validering
✅ Objektorienterad arkitektur med DRY-principer
✅ Chatthistorik som visas i fallande ordning (senaste högst upp)
✅ Mobile First design (porträtt)
✅ Environment variable för API-nyckel
✅ Fullständig felhantering
✅ Automatisk sparning av chatthistorik i `ChatFiles/`-mappen vid app-avslut
✅ Chatterbox persona (55 år, Stockholm, expert på hemelektronik)
✅ NetOnNet branding med färgprofil och logotyp

## 🏗️ Teknisk Stack

- **Backend:** ASP.NET Core 8.0 MVC
- **AI:** Azure OpenAI Service (GPT-4)
- **Frontend:** HTML5, CSS3, Vanilla JavaScript
- **Arkitektur:** MVC med Repository Pattern
- **Design:** Mobile First, Responsive Design



## 🔧 Installation & Konfiguration

### 1. Förutsättningar
- .NET 8.0 SDK
- Azure OpenAI-resurs med deployment
- Git

### 2. Klona projektet
```bash
git clone [https://github.com/maxiimize/ChatterboxApp.git]
cd Chatterbox
```

### 3. Konfigurera Azure OpenAI

#### A. Sätt environment variable (Windows)
```cmd
setx OPENAIKEY "Your-Api-Key"
```

#### B. Uppdatera `Services/AzureOpenAIService.cs`
```csharp
_endpoint = "https://chatterboxapp.openai.azure.com/";
_deploymentName = "chatterbox-gpt35n";
```

### 4. Kör applikationen
```bash
dotnet restore
dotnet run
```


## 🎨 Design & UX

### Färgpalett (NetOnNet)
- **Primär:** `#0095DA` (Ljusblå)
- **Sekundär:** `#00537C` (Mörkblå)
- **Accent:** `#E30613` (Röd)
- **Bakgrund:** `#F5F5F5` (Ljusgrå)
- **Text:** `#1A1A1A` (Nästan svart)

### Mobile First
Designen är optimerad för mobila enheter först och skalas sedan upp för tablets och desktop:
- **Mobil:** < 768px
- **Tablet:** 768px - 1023px
- **Desktop:** ≥ 1024px

## 🏛️ Arkitektur & Design Patterns

### 1. **Model-View-Controller (MVC)**
Separerar business logic, data och presentation.

### 2. **Dependency Injection**
Services registreras i `Program.cs` och injiceras i controllers:
```csharp
builder.Services.AddSingleton<IAzureOpenAIService, AzureOpenAIService>();
builder.Services.AddSingleton<ChatHistoryService>();
```

### 3. **Repository Pattern**
`ChatHistoryService` agerar som repository för chattdata.

### 4. **Interface Segregation**
`IAzureOpenAIService` definierar kontraktet för AI-tjänsten.

### 5. **Single Responsibility Principle (SRP)**
Varje klass har ett tydligt ansvar:
- `ChatMessage`: Representerar data
- `AzureOpenAIService`: AI-kommunikation
- `ChatHistoryService`: Historikhantering
- `ChatController`: API-endpoints

### 6. **DRY (Don't Repeat Yourself)**
Gemensam funktionalitet är extraherad till återanvändbara metoder.

## 🔒 Säkerhet & Validering

### Input-validering
- Max 2000 tecken per meddelande
- Required-attribut på alla inputs
- HTML-encoding av användarinput
- XSS-skydd via sanitering



## 📝 API-Endpoints

### `POST /api/chat/send`
Skickar ett meddelande och får svar från AI.

**Request:**
```json
{
  "message": "Hur stor TV ska jag köpa?"
}
```

**Response:**
```json
{
  "success": true,
  "userMessage": "Hur stor TV ska jag köpa?",
  "aiResponse": "Det beror på...",
  "timestamp": "2025-01-20T10:30:00",
  "chatHistory": [...]
}
```

### `GET /api/chat/history`
Hämtar chatthistorik i fallande ordning.

### `POST /api/chat/clear`
Rensar aktuell chattsession.

### `GET /api/chat/health`
Health check för tjänsten.

## 💾 Filhantering

### ChatFiles-mappen
Chatthistorik sparas automatiskt efter varje meddelande:
- Format: `chat_YYYY-MM-DD_HH-mm-ss_[session-id].json`
- Plats: `ChatFiles/` (samma nivå som Program.cs)
- JSON-format för enkel parsning

**Exempel:**
```json
{
  "sessionId": "abc123...",
  "messages": [
    {
      "role": "user",
      "content": "Hej!",
      "timestamp": "2025-01-20T10:00:00"
    }
  ],
  "createdAt": "2025-01-20T10:00:00",
  "lastUpdated": "2025-01-20T10:30:00"
}
```

## 🧪 Testning

### Manuell testning
1. **Tom input:** Försök skicka tomt meddelande → Felmeddelande visas
2. **Långt meddelande:** Skriv över 2000 tecken → Räknare blir röd, validering blockerar
3. **AI-svar:** Ställ fråga om hemelektronik → Får relevant svar
4. **Off-topic:** Fråga om mat → Chatterbox säger att han endast hjälper med hemelektronik
5. **Sortering:** Kontrollera att senaste meddelanden visas högst upp
6. **Rensa:** Klicka "Rensa chatt" → Historik töms
7. **Mobil:** Testa i olika skärmstorlekar → Responsiv design

## 🌐 Azure-länk

**Azure Web App:** [https://chatterboxapp.openai.azure.com/openai/deployments/chatterbox-gpt35/chat/completions?api-version=2025-01-01-preview]

## 📊 Git-strategi

### Branch-struktur
- `main` - Produktionsklar kod
- `develop` - Utvecklingsbranch
- `feature/*` - Feature-brancher



## 👥 Författare

**[Max Berridge]**
- GitHub: [@maxiimize](https://github.com/maxiimize)

## 📚 Lärdomar

### Tekniska lärdomar
- Azure OpenAI API-integration
- Real-time chat-uppdateringar
- Filhantering vid app-shutdown
- Mobile First CSS-utveckling

### Design Patterns
- MVC-arkitektur i ASP.NET Core
- Dependency Injection
- Repository Pattern
- Interface Segregation

### Best Practices
- Input-validering på både client och server
- Proper error handling med try-catch
- DRY-principer i kod
- Semantic HTML och tillgänglig design

## 📄 Licens

Detta projekt är skapat för utbildningssyfte.

---

**🎓 Skolprojekt för Edugrade**
**📅 Datum:** Oktober 2025

**OBS:** Projektet använder NetOnNets färgprofil och design som inspiration.
