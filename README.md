# Mythic Empire - Unity Client

## Mô tả
Dự án game thủ thành 3D của nhóm là một trò chơi chiến thuật thời gian thực, trong đó người chơi đóng vai trò làm thủ lĩnh, xây dựng các tháp phòng thủ, thả các binh lính để chống lại sự xâm lược của kẻ thù. Trong game, người chơi sẽ có cơ hội thiết kế và xây dựng các loại tòa nhà và cơ sở quân sự, tuyến phòng thủ và đào tạo quân lính (nâng cấp theo sao) với các kỹ năng và khả năng chiến đấu khác nhau. 

Dự án này là một trò chơi được phát triển bằng Unity, sử dụng công nghệ SignalR để tạo kết nối trực tiếp giữa các người chơi. Cơ sở dữ liệu được lưu trữ bằng MongoDB, và ASP.NET được sử dụng để xây dựng backend cho ứng dụng.
## Video Demo
Demo Video:[https://www.youtube.com/watch?v=RXJoQbxcVh4]
[![Video Thumbnail](https://img.youtube.com/vi/RXJoQbxcVh4/maxresdefault.jpg)](https://www.youtube.com/watch?v=RXJoQbxcVh4)



## Gameplay
### Giao diện sảnh chính trò chơi 
<p align="center">
<a href="https://i.ibb.co/87ZF7c7/Lobby.png"><img src="https://i.ibb.co/87ZF7c7/Lobby.png" alt="Lobby" border="0" width="1000"></a>
  </p>  
Các chức năng chính:
- Shop: Người chơi mua, bán các thẻ bài mà mình yêu thích
- Advanture: Chế độ chơi với máy, người chơi sẽ được luyện tập và chơi với máy được lập trình từ trước
- Arena: Chế độ chơi với người chơi khác, người chơi sẽ được thi đấu với các người chơi khác để nâng cao thứ hạng của bản thân.
- Gacha: Người chơi dùng tài nguyên của mình để có cơ hội nhận được các thẻ bài quý hiếm.

### Hệ thống thẻ bài
Để bắt đầu các trận chiến, người chơi phải có cho mình các thẻ bài riêng cho bản thân để xây dựng các chiến thuật phù hợp.
#### Các loại thẻ bài
<p align="center">
<table>
    <tr>
        <td align="center"><img src="https://i.ibb.co/zXMH0rz/golem-common-card.png" alt="Image 1" width="300"><br><strong>Thẻ quái</strong></td>
        <td align="center"><img src="https://i.ibb.co/GczYHdm/energy-common-card-1.png" alt="Image 2" width="300"><br><strong>Thẻ tháp</strong></td>
        <td align="center"><img src="https://i.ibb.co/5G6t0fY/toxic-common-card.png" alt="Image 3" width="300"><br><strong>Thẻ hỗ trợ</strong></td>
    </tr>
</table>
</p>
Có 3 loại thẻ bài chính đó là thẻ tháp, thẻ quái và thẻ hỗ trợ: 
- Thẻ tháp: cho phép xây dựng 1 đơn vị tháp lên sân đấu để phòng thủ hoặc hỗ trợ.
- Thẻ quái: triệu hồi 1 đơn vị lính tấn công thành của địch.
- Thẻ hỗ trợ: Cung cấp các hiệu ứng hỗ trợ trong phạm vi nhất định như hồi máu, đóng băng, tăng tốc ...

#### Độ hiếm của thẻ bài
Mỗi thẻ bài sẽ có độ hiếm tương ứng với sức mạnh tăng dần như sau: Common, Rare, Mythic, Legend
<p align="center">
<table>
    <tr>
        <td align="center"><img src="https://i.ibb.co/VvdkwVF/dog-common-card.png" alt="Image 1" width="300"><br><strong>Thẻ Common</strong></td>
        <td align="center"><img src="https://i.ibb.co/GvFpMB2/dog-rare-card.png" alt="Image 2" width="300"><br><strong>Thẻ Rare</strong></td>
        <td align="center"><img src="https://i.ibb.co/NCM4DTx/dog-mythic-card.png" alt="Image 3" width="300"><br><strong>Thẻ Mythic</strong></td>
        <td align="center"><img src="https://i.ibb.co/Pmw722x/dog-legend-card.png" alt="Image 4" width="300"><br><strong>Thẻ Legend</strong></td>
    </tr>
</table>
</p>
#### Cấp độ của thẻ bài
Ngoài việc tìm kiếm các thẻ bài có độ hiếm cao để gia tăng sức mạnh, người chơi cũng có thể nâng cấp các thẻ bài để chúng trở nên mạnh hơn, gia tăng sức mạnh đội hình của bạn.
<p align="center">
    <div>
        <img src="https://i.ibb.co/J35J4D3/Group-61.png" alt="Image 1">
        <h2>Cấm độ thẻ bài tăng dần</h2>
    </div>
</p>
### Giao diện trò chơi
Khi bắt đầu trận đấu, bản đồ sẽ được khởi tạo với những vật cản như cây cối, người chơi không thể đặt các thẻ bài trên các vật cản đó.

<p align="center">
<a href="https://i.ibb.co/5WrC5pX/start-gameplay.png"><img src="https://i.ibb.co/5WrC5pX/start-gameplay.png" alt="Lobby" border="0" width="1000"></a>
</p>  
Người chơi kéo thả các thẻ bài từ thanh thẻ bài bên dưới để đặt các thẻ bài lên sân đấu. Người chơi phải xây dựng chiến thuật phòng thủ mạnh mẽ trước các đợt tấng công của kẻ thù đồng thời tổ chức các đợt tấn công đối thủ để dành được chiến thắng.

<p align="center">
<a href="https://i.ibb.co/Vt7j4KF/ingame.png"><img src="https://i.ibb.co/Vt7j4KF/ingame.png" alt="Lobby" border="0" width="1000"></a>
</p> 
## Yêu cầu
- Unity 2021.3.1f1
- SignalR
- MongoDB version 6
- ASP.NET version 6

## Cài đặt 
1. clone repo
2. Khởi chạy bằng unity
3. Chạy các dự án backend server và realtime server cần thiết. Đảm bảo bạn đã cài đặt và cấu hình chúng đúng cách.
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


