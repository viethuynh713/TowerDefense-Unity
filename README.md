# Mythic Empire - Thủ thành đối kháng tự do 3D

## Video Demo
[Watch Demo Video](https://www.youtube.com/watch?v=RXJoQbxcVh4)
![Gameplay](https://i.ibb.co/Vt7j4KF/ingame.png)



## Mô tả
Dự án game thủ thành 3D của nhóm là một trò chơi chiến thuật thời gian thực, trong đó người chơi đóng vai trò làm thủ lĩnh, xây dựng các tháp phòng thủ, thả các binh lính để chống lại sự xâm lược của kẻ thù. Trong game, người chơi sẽ có cơ hội thiết kế và xây dựng các loại tòa nhà và cơ sở quân sự, tuyến phòng thủ và đào tạo quân lính (nâng cấp theo sao) với các kỹ năng và khả năng chiến đấu khác nhau. 

Dự án này là một trò chơi được phát triển bằng Unity, sử dụng công nghệ SignalR để tạo kết nối trực tiếp giữa các người chơi. Cơ sở dữ liệu được lưu trữ bằng MongoDB, và ASP.NET được sử dụng để xây dựng backend cho ứng dụng.

## Công Nghệ
- Unity
- SignalR
- MongoDB
- ASP.NET

3. Chạy các dự án backend và realtime cần thiết. Đảm bảo bạn đã cài đặt và cấu hình chúng đúng cách.
   - Backend project: [https://github.com/viethuynh713/TowerDefense-Backend]
   - Realtime project: [https://github.com/viethuynh713/TowerDefense_Realtime]
4. Config các đường dẫn tới các dự án trong file `NetworkingConfig.cs`. Dưới đây là một ví dụ về cách cấu hình:
   ```csharp
   // NetworkingConfig.cs
   public class NetworkingConfig {
       public const string ServiceURL = "http://localhost:5000/api";
       public const string RealtimeURL = "ws://localhost:8080"; 
   }



## Đóng Góp
Chúng tôi rất hoan nghênh mọi đóng góp từ cộng đồng. Hãy tạo một Pull Request để đóng góp vào dự án này.

## Tác Giả

Liên Hệ: viethuynh713@gmail.com


