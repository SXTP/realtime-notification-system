# Realtime Notification System (Distributed SignalR)

Bu proje, mikroservis mimarilerinde ölçeklenebilir ve gerçek zamanlı bir bildirim sisteminin nasıl kurulacağını gösteren bir **Production Pattern** örneğidir.

## 🚀 Özellikler

- **ASP.NET Core 9.0 SignalR**: WebSocket tabanlı çift yönlü iletişim.
- **Redis Pub/Sub Backplane**: Birden fazla backend instance'ının birbiriyle haberleşmesini sağlar.
- **Horizontal Scaling**: Uygulama istenildiği kadar kopyalanabilir, mesaj iletimi bozulmaz.
- **Docker Compose**: Tüm altyapıyı (API + Redis) tek komutla ayağa kaldırır.

## 🏗️ Mimari Yapı

Sistem, SignalR'ın varsayılan "bellek içi" mesajlaşma yapısını **Redis Backplane** ile değiştirir. Bir kullanıcı Sunucu-A'ya bağlıyken, bildirim isteği Sunucu-B'ye gelse bile Redis sayesinde mesaj tüm sunuculara yayılır ve ilgili kullanıcıya ulaştırılır.

[Image of SignalR Redis backplane architecture]

## 🛠️ Kurulum ve Çalıştırma

Projenin çalışması için bilgisayarınızda **Docker Desktop** yüklü olmalıdır.

1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/kullaniciadiniz/realtime-notification-system.git
   cd realtime-notification-system
   ```

2. Sistemi ayağa kaldırın:
   ```bash
   docker-compose up --build
   ```

Bu komut şunları başlatacaktır:
- 1 adet Redis Container (Port: 6379)
- 3 adet Notification.Api Instance (Portlar: 5001, 5002, 5003)

## 🚦 Test Adımları

1. **Bağlantı Kur**: Tarayıcıdan `http://localhost:5001/notifications` adresine bir WebSocket bağlantısı açın.
2. **Mesaj Gönder**: Postman kullanarak **farklı bir porttan** (örn: 5003) tetikleme yapın:

   - **URL**: `POST http://localhost:5003/api/notify`
   - **Gövde (JSON)**:
     ```json
     {
       "UserId": "all",
       "Message": "Merhaba Dağıtık Sistemler!",
       "Type": "Info"
     }
     ```
3. **Sonuç**: Mesajın 5001 portundaki client'a ulaştığını göreceksiniz.

## 🔍 Sağlık Kontrolü
Uygulamanın durumunu takip etmek için:
`http://localhost:5001/health`

## 🛣️ Gelecek Geliştirmeler
- [ ] Nginx Load Balancer entegrasyonu (Sticky Sessions).
- [ ] JWT Authentication ile User-specific bildirimler.
- [ ] PostgreSQL ile bildirim geçmişi saklama.
