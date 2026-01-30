# 🤖 Chatterbox - NetOnNet AI Customer Service

An AI-driven chatbot built with ASP.NET Core MVC and Azure OpenAI to help customers with home electronics.

## 📋 Project Description

Chatterbox is an intelligent chatbot who works at NetOnNet's customer service. He is 55 years old, lives in Stockholm and is an expert in home electronics. The chatbot uses Azure OpenAI to provide helpful and professional answers to questions about TVs, computers, mobile phones, audio equipment, household appliances and other home electronics.

## 🚀 Features

✅ Secure input handling with validation
✅ Object-oriented architecture with DRY principles
✅ Chat history displayed in descending order (latest on top)
✅ Mobile First design (portrait)
✅ Environment variable for API key
✅ Complete error handling
✅ Automatic saving of chat history in `ChatFiles/` folder on app shutdown
✅ Chatterbox persona (55 years old, Stockholm, home electronics expert)
✅ NetOnNet branding with color profile and logo

## 🏗️ Technical Stack

- **Backend:** ASP.NET Core 8.0 MVC
- **AI:** Azure OpenAI Service (GPT-4)
- **Frontend:** HTML5, CSS3, Vanilla JavaScript
- **Architecture:** MVC with Repository Pattern
- **Design:** Mobile First, Responsive Design



## 🔧 Installation & Configuration

### 1. Prerequisites
- .NET 8.0 SDK
- Azure OpenAI resource with deployment
- Git

### 2. Clone the project
```bash
git clone [https://github.com/maxiimize/ChatterboxApp.git]
cd Chatterbox
```

### 3. Configure Azure OpenAI

#### A. Set environment variable (Windows)
```cmd
setx OPENAIKEY "Your-Api-Key"
```

#### B. Update `Services/AzureOpenAIService.cs`
```csharp
_endpoint = "https://chatterboxapp.openai.azure.com/";
_deploymentName = "chatterbox-gpt35n";
```

### 4. Run the application
```bash
dotnet restore
dotnet run
```


## 🎨 Design & UX

### Color Palette (NetOnNet)
- **Primary:** `#0095DA` (Light Blue)
- **Secondary:** `#00537C` (Dark Blue)
- **Accent:** `#E30613` (Red)
- **Background:** `#F5F5F5` (Light Grey)
- **Text:** `#1A1A1A` (Almost Black)

### Mobile First
The design is optimized for mobile devices first and then scales up for tablets and desktop:
- **Mobile:** < 768px
- **Tablet:** 768px - 1023px
- **Desktop:** ≥ 1024px

## 🏛️ Architecture & Design Patterns

### 1. **Model-View-Controller (MVC)**
Separates business logic, data and presentation.

### 2. **Dependency Injection**
Services are registered in `Program.cs` and injected into controllers:
```csharp
builder.Services.AddSingleton<IAzureOpenAIService, AzureOpenAIService>();
builder.Services.AddSingleton<ChatHistoryService>();
```

### 3. **Repository Pattern**
`ChatHistoryService` acts as a repository for chat data.

### 4. **Interface Segregation**
`IAzureOpenAIService` defines the contract for the AI service.

### 5. **Single Responsibility Principle (SRP)**
Each class has a clear responsibility:
- `ChatMessage`: Represents data
- `AzureOpenAIService`: AI communication
- `ChatHistoryService`: History management
- `ChatController`: API endpoints

### 6. **DRY (Don't Repeat Yourself)**
Common functionality is extracted into reusable methods.

## 🔒 Security & Validation

### Input validation
- Max 2000 characters per message
- Required attribute on all inputs
- HTML encoding of user input
- XSS protection via sanitization



## 📝 API Endpoints

### `POST /api/chat/send`
Sends a message and receives a response from the AI.

**Request:**
```json
{
  "message": "What size TV should I buy?"
}
```

**Response:**
```json
{
  "success": true,
  "userMessage": "What size TV should I buy?",
  "aiResponse": "It depends on...",
  "timestamp": "2025-01-20T10:30:00",
  "chatHistory": [...]
}
```

### `GET /api/chat/history`
Retrieves chat history in descending order.

### `POST /api/chat/clear`
Clears the current chat session.

### `GET /api/chat/health`
Health check for the service.

## 💾 File Management

### ChatFiles folder
Chat history is automatically saved after each message:
- Format: `chat_YYYY-MM-DD_HH-mm-ss_[session-id].json`
- Location: `ChatFiles/` (same level as Program.cs)
- JSON format for easy parsing

**Example:**
```json
{
  "sessionId": "abc123...",
  "messages": [
    {
      "role": "user",
      "content": "Hello!",
      "timestamp": "2025-01-20T10:00:00"
    }
  ],
  "createdAt": "2025-01-20T10:00:00",
  "lastUpdated": "2025-01-20T10:30:00"
}
```

## 🧪 Testing

### Manual testing
1. **Empty input:** Try sending empty message → Error message is displayed
2. **Long message:** Write over 2000 characters → Counter turns red, validation blocks
3. **AI response:** Ask question about home electronics → Receives relevant answer
4. **Off-topic:** Ask about food → Chatterbox says he only helps with home electronics
5. **Sorting:** Verify that latest messages are displayed on top
6. **Clear:** Click "Clear chat" → History is cleared
7. **Mobile:** Test in different screen sizes → Responsive design

## 🌐 Azure Link

**Azure Web App:** [https://chatterboxapp.openai.azure.com/openai/deployments/chatterbox-gpt35/chat/completions?api-version=2025-01-01-preview]

## 📊 Git Strategy

### Branch structure
- `main` - Production-ready code
- `develop` - Development branch
- `feature/*` - Feature branches



## 👥 Author

**[Max Berridge]**
- GitHub: [@maxiimize](https://github.com/maxiimize)

## 📚 Lessons Learned

### Technical lessons
- Azure OpenAI API integration
- Real-time chat updates
- File management on app shutdown
- Mobile First CSS development

### Design Patterns
- MVC architecture in ASP.NET Core
- Dependency Injection
- Repository Pattern
- Interface Segregation

### Best Practices
- Input validation on both client and server
- Proper error handling with try-catch
- DRY principles in code
- Semantic HTML and accessible design

## 📄 License

This project was created for educational purposes.

---

🎓 School project for Edugrade
📅 October 2025
