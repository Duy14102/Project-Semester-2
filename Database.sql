drop database if exists OrderDB;

create database OrderDB;

use OrderDB;

create table Customers(
	customer_id int auto_increment primary key,
    customer_name varchar(100) not null,
    customer_address varchar(200)
);

create table Items(
	item_id int auto_increment primary key,
    item_name varchar(200) not null,
    unit_price decimal(20,2) default 0,
    amount int not null default 0,
    category varchar(500),
    item_story varchar(500),
    item_status tinyint not null, 
    item_description varchar(500)
);

create table Orders(
	order_id int auto_increment primary key,
    order_date datetime default now() not null,
    customer_id int,
    order_status int,
    constraint fk_Orders_Customers foreign key(customer_id) references Customers(customer_id)
);

create table OrderDetails(
	order_id int not null,
    item_id int not null,
    unit_price decimal(20,2) not null,
    quantity int not null default 1,
    constraint pk_OrderDetails primary key(order_id, item_id),
    constraint fk_OrderDetails_Orders foreign key(order_id) references Orders(order_id),
    constraint fk_OrderDetails_Items foreign key(item_id) references Items(item_id)
);

create table Feedbacks(
	feedback_id int auto_increment primary key,
	feedback_name varchar(500),
    feedback_story varchar(500),
    feedback_link varchar(500) default '~/image/DefaultFeedback.jpg',
    feedback_date datetime default now() not null
);

SELECT DATE_FORMAT(feedback_date, 'Ngày %d Tháng %m Năm %Y') FROM Feedbacks;

delimiter $$
create trigger tg_before_insert before insert
	on Items for each row
    begin
		if new.amount < 0 then
            signal sqlstate '45001' set message_text = 'tg_before_insert: amount must > 0';
        end if;
    end $$
delimiter ;

delimiter $$
create trigger tg_CheckAmount
	before update on Items
	for each row
	begin
		if new.amount < 0 then
            signal sqlstate '45001' set message_text = 'tg_CheckAmount: amount must > 0';
        end if;
    end $$
delimiter ;

delimiter $$
create procedure sp_createCustomer(IN customerName varchar(100), IN customerAddress varchar(200), OUT customerId int)
begin
	insert into Customers(customer_name, customer_address) values (customerName, customerAddress); 
    select max(customer_id) into customerId from Customers;
end $$
delimiter ;

call sp_createCustomer('no name','any where', @cusId);
select @cusId;

/* INSERT DATA */
insert into Customers(customer_name, customer_address) values
	('Nguyen Thi X','Hai Duong'),
    ('Nguyen Van N','Hanoi'),
    ('Nguyen Van B','Ho Chi Minh'),
    ('Nguyen Van A','Hanoi');
select * from Customers;

insert into Feedbacks(feedback_link,feedback_name, feedback_story) values
('~/image/FeedBack1.jpg','Bùi Xuân Huấn', 'Đồ ăn ở đây các món cũng giản dị thôi, nhưng mình ăn lại rất hợp, kiểu cơm nhà. Ăn các chỗ khác sẽ nêm nếm nhiều gia vị, rồi có nhiều sốt các thứ, nhưng ở đây món ăn khá đơn giản và mộc mạc. Cơm và thức ăn đầy đặn, được cái nhiều rau, thường có thêm tráng miệng là hoa quả.'),
('~/image/FeedBack2.jpg','Ngô Bá Khá', 'Bên này là chân ái của mình nè, ở đây mô hình bán cơm hơi khác, đầu tiên là người nấu là các mẹ nội trợ ý, thấy bảo start-up khởi nghiệp gì, trước lên báo cũng nổi lắm. Hai là họ thiên về bán theo gói cơm, cũng có bán lẻ theo suất nhưng hình như cx đắt hàng nên nhanh hết.'),
('~/image/FeedBack3.jpg','Nguyễn Thành Tiến', 'Hương vị món ăn ngon, nêm nếm ổn. Phong độ nấu có vẻ là ổn định nhất trong 3 bên. Đồ ăn cũng được đầy đặn. À có cả thực đơn eat clean cho chị em nào muốn giảm cân ạ, ăn gạo lứt hoặc bánh mì ngũ cốc ý :)) tất nhiên giá cũng chát, tầm 60k/suất. Công nhận là cơm ngon hơn cơm tù.');
select * from Feedbacks;


insert into Items(item_name, unit_price, category, item_status, item_description, item_story) values
	('Cơm Trắng', 10.0, 'Đồ Ăn Kèm', 1, '~/image/ComTrang.jpg', 'Bạn nên ăn cơm hàng ngày vì nó rất giàu vitamin D, niacin, canxi, chất xơ, riboflavin, sắt và thiamine. Tất cả các thành phần dinh dưỡng này đều cần thiết cho cơ thể bạn để thúc đẩy hệ miễn dịch và giúp cân bằng các hoạt động chung của cơ thể.'),
    ('Đậu Hũ', 10.0, 'Đồ Ăn Kèm', 1, '~/image/DauHu.jpg', 'Đậu phụ là nguồn selen dồi dào, khoáng chất cần thiết cho cơ thể để giúp hệ thống chống oxy hóa hoạt động đúng đắn, từ đó ngăn ngừa ung thư đường ruột. Đàn ông cũng có thể ngăn ngừa ung thư tuyến tiền liệt bằng cách ăn đậu phụ, nhưng với số lượng vừa phải.'),
    ('Dưa Xào Lòng', 30.0, 'Thịt', 1, '~/image/DuaXaoLong.jpg', 'Sự kết hợp giữa lòng non (lòng già, tràng lợn) với dưa thành món ăn ngon giòn chua chua mặn măn, dai dai, giòn giòn ai ăn cũng khoái khẩu - nhất là đàn ông ham nhậu. Món ăn này làm rất đơn giản, rất ngon miệng, rẻ tiền, vừa dễ ăn với cơm vừa dùng làm "mồi nhậu".'),
    ('Vịt Quay', 50.0, 'Thịt', 1, '~/image/VitQuay.png', 'Theo các nhà thuốc đông y thì thịt vịt có tác dụng dưỡng âm. Các bệnh về tiểu tiện bất lợi, lao phổi, nhiệt cơ thể nếu sử dụng thịt vịt để bồi bổ sẽ rất tốt. Thịt vịt đực càng có tác dụng tốt hơn trong việc chữa bệnh.'),
    ('Cà Muối', 5.0, 'Đồ Ăn Kèm', 1, '~/image/CaMuoi.jpg', 'Cà có vị ngọt, tính hàn; có tác dụng nhuận tràng, lợi tiểu, tiêu thũng, trừ ôn dịch, hoạt huyết, tiêu viêm chỉ thống; cà là tác nhân kích thích chuyển hóa cholesterol ở gan. Cà có một alcaloid độc, có nhiều khi quả còn non xanh, nên Hải Thượng Lãn Ông khuyên không ăn nhiều cà sống.'),
    ('Dưa Muối', 5.0, 'Đồ Ăn Kèm', 1, '~/image/DuaMuoi.jpg', 'Dưa muối chứa rất nhiều vitamin, khoáng chất trong nước muối ngâm giấm của chúng cũng như là nguồn bổ sung chất chống oxy hóa tuyệt vời, có thể trung hòa các gốc tự do, làm chậm quá trình lão hóa và giảm nguy cơ mắc các bệnh lý liên quan đến tim mạch.'),
    ('Canh Cua', 10.0, 'Canh', 1, '~/image/CanhCua.jpg', 'Canh cua đồng được xem là món ăn có tác dụng giải nhiệt hiệu quả trong mùa hè. Canh cua đồng được xem là món ăn tốt đối với trẻ còi xương và người bị loãng xương. Theo đông y, cua đồng có vị mặn, tính hàn, hơi độc, có tác dụng sinh phong liền gân nối xương.'),
    ('Canh Bí', 7.0, 'Canh', 1, '~/image/CanhBi.jpg', 'Trong bí xanh chứa rất nhiều kali. Kali có khả năng làm giãn mạch cũng như giảm sự căng thẳng trên mạch máu và động mạch, giúp ngăn ngừa các vấn đề liên quan đến tim mạch như đột quỵ và đau thắt cơ tim.'),
    ('Cocacola', 10.0, 'Nước', 1, '~/image/Cocacola.jpg', 'Thức uống này có công hiệu gần như gừng trong điều trị buồn nôn. Vậy nên, nếu bị chứng buồn nôn gây khó chịu, hãy nhấm nháp một chút nước ngọt Cocacola, chứng buồn nôn của bạn sẽ được cải thiện đáng kể.'),
    ('Sprite', 10.0, 'Nước', 1, '~/image/Sprite.jpg', 'Sprite cũng được sử dụng để điều trị tắc nghẽn dạ dày. Các axit trong thức uống này tương tự như axit dạ dày; giúp tiêu hóa chất xơ và phá hủy các tắc nghẽn, nhấm nháp một chút Sprite ở nhiệt độ phòng sẽ làm dịu cổ họng.'),
    ('Bia Các Loại', 10.0, 'Nước', 1, '~/image/Bia.jpg', 'Bia nhẹ chỉ có khoảng hai phần ba lượng calo của bia thông thường và ít cồn hơn một chút. Mặc dù bia chứa một lượng nhỏ vi chất dinh dưỡng, nhưng đây không phải là nguồn tốt so với thực phẩm nguyên chất như trái cây và rau quả.'),
    ('Lợn Luộc', 7.5, 'Thịt', 1, '~/image/LonLuoc.jpg', 'Thịt lợn giúp đóng góp vào cơ thể rất nhiều loại vitamin và khoáng chất khác nhau như photpho, kali, nicaxin, vitamin B6, vitamin B12, kẽm... Trong đó hàm lượng vitamin B có trong thịt lợn là nguồn vitamin chính mà con người nhận từ thực phẩm.');
select * from Items;

insert into Orders(customer_id, order_status) values
	(1, 1), (2, 1), (1, 1);
select * from Orders;

insert into OrderDetails(order_id, item_id, unit_price, quantity) values
	(1, 1, 12.5, 5), (1, 3, 31.0, 1), (1, 4, 24.5, 2),
    (2, 2, 62.5, 1), (2, 3, 31.0, 2), (2, 5, 7.5, 4);
select * from OrderDetails;

/* CREATE & GRANT USER */
create user if not exists 'vtca'@'localhost' identified by 'vtcacademy';
grant all on OrderDB.* to 'vtca'@'localhost';
-- grant all on Items to 'vtca'@'localhost';
-- grant all on Customers to 'vtca'@'localhost';
-- grant all on Orders to 'vtca'@'localhost';
-- grant all on OrderDetails to 'vtca'@'localhost';

select item_id from Items order by item_id desc limit 1;

select customer_id, customer_name,
    ifnull(customer_address, '') as customer_address
from Customers where customer_id=1;
                        
select order_id from Orders order by order_id desc limit 1;

select LAST_INSERT_ID();
select customer_id from Customers order by customer_id desc limit 1;

update Items set amount=10 where item_id=3;
-- lock table Orders write;
-- unlock tables;