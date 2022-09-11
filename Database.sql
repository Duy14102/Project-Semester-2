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

create table Feedback(
	feedback_id int auto_increment primary key,
	feedback_name varchar(500),
    feedback_story varchar(500)
);
select * from Feedback;

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

insert into Feedback(feedback_name, feedback_story) values
('Bùi Xuân Huấn', 'Đồ ăn ở đây các món cũng giản dị thôi, nhưng mình ăn lại rất hợp, kiểu cơm nhà. Ăn các chỗ khác sẽ nêm nếm nhiều gia vị, rồi có nhiều sốt các thứ, nhưng ở đây món ăn khá đơn giản và mộc mạc. Cơm và thức ăn đầy đặn, được cái nhiều rau, thường có thêm tráng miệng là hoa quả.'),
('Ngô Bá Khá', 'Bên này là chân ái của mình nè, ở đây mô hình bán cơm hơi khác, đầu tiên là người nấu là các mẹ nội trợ ý, thấy bảo start-up khởi nghiệp gì, trước lên báo cũng nổi lắm. Hai là họ thiên về bán theo gói cơm, cũng có bán lẻ theo suất nhưng hình như cx đắt hàng nên nhanh hết.'),
('Nguyễn Thành Long', 'Hương vị món ăn ngon, nêm nếm ổn. Phong độ nấu có vẻ là ổn định nhất trong 3 bên. Đồ ăn cũng được đầy đặn. À có cả thực đơn eat clean cho chị em nào muốn giảm cân ạ, ăn gạo lứt hoặc bánh mì ngũ cốc ý :)) tất nhiên giá cũng chát, tầm 60k/suất.');

insert into Items(item_name, unit_price, category, item_status, item_description) values
	('Cơm Trắng', 10.0, 'Đồ Ăn Kèm', 1, '~/image/ComTrang.jpg'),
    ('Đậu Hũ', 10.0, 'Đồ Ăn Kèm', 1, '~/image/DauHu.jpg'),
    ('Dưa Xào Lòng', 30.0, 'Thịt', 1, '~/image/DuaXaoLong.jpg'),
    ('Vịt Quay', 50.0, 'Thịt', 1, '~/image/VitQuay.png'),
    ('Cà Muối', 5.0, 'Đồ Ăn Kèm', 1, '~/image/CaMuoi.jpg'),
    ('Dưa Muối', 5.0, 'Đồ Ăn Kèm', 1, '~/image/DuaMuoi.jpg'),
    ('Canh Cua', 10.0, 'Canh', 1, '~/image/CanhCua.jpg'),
    ('Canh Bí', 7.0, 'Canh', 1, '~/image/CanhBi.jpg'),
    ('Cocacola', 10.0, 'Nước', 1, '~/image/Cocacola.jpg'),
    ('Sprite', 10.0, 'Nước', 1, '~/image/Sprite.jpg'),
    ('Bia Các Loại', 10.0, 'Nước', 1, '~/image/Bia.jpg'),
    ('Lợn Luộc', 7.5, 'Thịt', 1, '~/image/LonLuoc.jpg');
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