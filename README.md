### Quan trọng
#### Lỗi "fatal"
Nếu xảy ra "fatal" khi dùng git thì hãy đưa ngữ cảnh cụ thể cho chat AI để nó giải quyết đúng nhé. Còn không thì khi họp, tôi sẽ xem xét qua.
#### .gitignore
Để ý file .gitignore của tôi nhé, đừng push những gì không cần thiết đến dự án và không có thay đổi gì trong đó.
### Lưu khi dùng Git
#### git stash
- Dùng "git stash" lại trước khi pull về. Sau khi pull về xong, dùng "git stash pop" để lấy lại các thay đổi mấy bạn đã thực hiện trước khi pull về.
- Lý do tại sao tôi bảo các bạn dùng "git stash" là vì có các file tôi đã thay đổi ở trên remote được theo dõi (tracked). Khi các bạn pull về, git sẽ đồng bộ local repo của các bạn với remote repo, với file chưa được theo dõi (untracked) thì vẫn nguyên. Còn với file đã tracked thì nó sẽ xóa hết những gì các bạn làm ở những file đó và thay thế bằng nội dung trong file từ remote.

#### git pull và git rebase
- "git pull origin master" sẽ có lịch sử commit về merge. Còn "git rebase origin master" sẽ không để lịch sử commit về merge. Trông sạch hơn.
- Cả 2 cách đều có số lượng commit ở branch bằng nhau. 

#### Quy tắc git push
- Không push trực tiếp vào main hay develop. Tôi sẽ so sánh rồi marge vào develop. Nhánh main để khi hoàn thành trọn vẹn sẽ được merge vào.
- Khi các bạn làm chức năng hay giao diện, hãy tạo nhánh đúng với việc làm của các bạn. Nếu giao diện đăng nhập thì là ui/login, hay chức năng thì là feature/login. Sau đó commit lại rồi đẩy lên nhánh với tên tương ứng đó.
- Kế đến, các bạn sẽ thấy thông báo compare & pull request thì ấn vào rồi gửi PR (Pull Request) để tôi xem xét và duyệt nhé.
### Quy tắc mã nguồn
#### Về Layout
- Đối với _Layout.cshtml, tôi sẽ dùng để làm Layout trang chủ. Nếu giao diện của các bạn có cùng giao diện trang chủ về Layout thì dùng nhé. Còn nếu khác giao diện thì hãy tạo một Layout riêng theo quy tắc đặt tên: _Layout + tên Controller. Đặt Layout đó trong đường dẫn: /Views/Shared. 
- Vì tôi không biết các bạn đã làm gì với _Layout nên khi pull về thì git sẽ lấy _Layout của tôi và xóa nội dung trong _Layout của các bạn. Nên các bạn hãy copy lại nội dung và tạo file Layout theo quy tắc trên, sau đó dán vào đó nhé. 
- Để sử dụng Layout cho Controller của các bạn thì đưa vào hàm khởi tạo controller cái này: ViewData["Layout"] = "_Layout + tên controller". Còn nếu các bạn muốn dùng Layout cho riêng action thì ghi đè lên ở bên View đó tương tự như controller nhé.

#### CSS riêng
- Ở thẻ head của các Layout các bạn dùng, hãy thêm đoạn @RenderSecion("Head", required: false). Và với mỗi View có css riêng thì thêm @section Head {
    thẻ link với src là css tương ứng}. 
- Có bao nhiêu file css đưa bấy nhiêu link nhé.

