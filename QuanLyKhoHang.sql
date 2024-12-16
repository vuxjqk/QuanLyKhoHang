USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyKhoHang')
BEGIN
	ALTER DATABASE QuanLyKhoHang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE QuanLyKhoHang;
END;

-- Tạo cơ sở dữ liệu mới
CREATE DATABASE QuanLyKhoHang;
GO

USE QuanLyKhoHang;
GO

-- Tạo bảng dbo.DanhMuc
CREATE TABLE dbo.DanhMuc (
    MaDanhMuc INT IDENTITY PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Tạo bảng dbo.SanPham
CREATE TABLE dbo.SanPham (
    MaSanPham INT IDENTITY PRIMARY KEY,
    TenSanPham NVARCHAR(100) NOT NULL UNIQUE,
    MaDanhMuc INT NOT NULL,
    GiaXuat DECIMAL(10, 0) NOT NULL CHECK (GiaXuat > 0),
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0),
    DonVi NVARCHAR(10) NOT NULL,
    FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DanhMuc(MaDanhMuc)
);
GO

-- Tạo bảng dbo.NhanVien
CREATE TABLE dbo.NhanVien (
    MaNhanVien INT IDENTITY PRIMARY KEY,
    TenNhanVien NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(10) NOT NULL UNIQUE,
    DiaChi NVARCHAR(500) NOT NULL
);
GO

-- Tạo bảng dbo.NhaCungCap
CREATE TABLE dbo.NhaCungCap (
    MaNhaCungCap INT IDENTITY PRIMARY KEY,
    TenNhaCungCap NVARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(10) NOT NULL UNIQUE,
    DiaChi NVARCHAR(500) NOT NULL
);
GO

-- Tạo bảng dbo.KhachHang
CREATE TABLE dbo.KhachHang (
    MaKhachHang INT IDENTITY PRIMARY KEY,
    TenKhachHang NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(10) NOT NULL UNIQUE,
    DiaChi NVARCHAR(500) NOT NULL
);
GO

-- Tạo bảng dbo.PhieuNhap
CREATE TABLE dbo.PhieuNhap (
    MaPhieuNhap INT IDENTITY PRIMARY KEY,
    MaNhaCungCap INT NOT NULL,
    MaNhanVien INT NOT NULL,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(10, 0) NOT NULL CHECK (TongTien >= 0) DEFAULT 0,
    TrangThai NVARCHAR(50) NOT NULL,
    FOREIGN KEY (MaNhaCungCap) REFERENCES dbo.NhaCungCap(MaNhaCungCap),
    FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
);
GO

-- Tạo bảng dbo.PhieuXuat
CREATE TABLE dbo.PhieuXuat (
    MaPhieuXuat INT IDENTITY PRIMARY KEY,
    MaKhachHang INT NOT NULL,
    MaNhanVien INT NOT NULL,
    NgayXuat DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(10, 0) NOT NULL CHECK (TongTien >= 0) DEFAULT 0,
    TrangThai NVARCHAR(50) NOT NULL,
    FOREIGN KEY (MaKhachHang) REFERENCES dbo.KhachHang(MaKhachHang),
    FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
);
GO

-- Tạo bảng dbo.ChiTietPhieuNhap
CREATE TABLE dbo.ChiTietPhieuNhap (
    MaPhieuNhap INT NOT NULL,
    MaSanPham INT NOT NULL,
    DonGia DECIMAL(10, 0) NOT NULL CHECK (DonGia > 0),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    PRIMARY KEY (MaPhieuNhap, MaSanPham),
    FOREIGN KEY (MaPhieuNhap) REFERENCES dbo.PhieuNhap(MaPhieuNhap),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham)
);
GO

-- Tạo bảng dbo.ChiTietPhieuXuat
CREATE TABLE dbo.ChiTietPhieuXuat (
    MaPhieuXuat INT NOT NULL,
    MaSanPham INT NOT NULL,
    DonGia DECIMAL(10, 0) NOT NULL CHECK (DonGia >= 0) DEFAULT 0,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    PRIMARY KEY (MaPhieuXuat, MaSanPham),
    FOREIGN KEY (MaPhieuXuat) REFERENCES dbo.PhieuXuat(MaPhieuXuat),
    FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham(MaSanPham)
);
GO

-- Tạo bảng dbo.TaiKhoan
CREATE TABLE dbo.TaiKhoan (
    TenDangNhap VARCHAR(30) PRIMARY KEY,
    MatKhau VARCHAR(30) NOT NULL,
    VaiTro INT NOT NULL,
    MaNhanVien INT NOT NULL UNIQUE,
    FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien(MaNhanVien)
);
GO

-- Thêm dữ liệu vào bảng dbo.DanhMuc
INSERT INTO dbo.DanhMuc (TenDanhMuc) VALUES
    (N'Vật liệu xây dựng'),
    (N'Vật liệu hoàn thiện'),
    (N'Công cụ dụng cụ'),
    (N'Hóa chất'),
    (N'Thiết bị xây dựng'),
    (N'Vật liệu cách nhiệt');
GO

-- Thêm dữ liệu vào bảng dbo.SanPham
INSERT INTO dbo.SanPham (TenSanPham, MaDanhMuc, GiaXuat, SoLuongTon, DonVi) VALUES
    -- Vật liệu xây dựng
    (N'Xi măng', 1, 200000, 100, N'Bao'),
    (N'Gạch đất nung', 1, 15000, 200, N'Viên'),
    (N'Thép cuộn', 1, 30000, 50, N'Kg'),
    (N'Gỗ thông', 1, 5000000, 30, N'M3'),
    
    -- Vật liệu hoàn thiện
    (N'Sơn nước', 2, 250000, 50, N'Lit'),
    (N'Gạch men', 2, 100000, 100, N'M2'),
    (N'Giấy dán tường', 2, 50000, 200, N'M2'),
    (N'Phào chỉ', 2, 30000, 150, N'M'),
    
    -- Công cụ dụng cụ
    (N'Máy khoan', 3, 700000, 20, N'Chiếc'),
    (N'Kéo cắt', 3, 200000, 30, N'Chiếc'),
    (N'Máy cắt gạch', 3, 1000000, 10, N'Chiếc'),
    (N'Tua vít', 3, 10000, 100, N'Chiếc'),
    
    -- Hóa chất
    (N'Hóa chất tẩy rửa', 4, 50000, 60, N'Lit'),
    (N'Phân bón', 4, 30000, 100, N'Kg'),
    (N'Hóa chất xây dựng', 4, 200000, 50, N'Kg'),
    (N'Hóa chất bảo vệ gỗ', 4, 150000, 40, N'Lit'),
    
    -- Thiết bị xây dựng
    (N'Máy xúc', 5, 8000000, 5, N'Chiếc'),
    (N'Máy trộn bê tông', 5, 12000000, 4, N'Chiếc'),
    (N'Xe tải', 5, 50000000, 3, N'Chiếc'),
    (N'Máy cẩu', 5, 60000000, 2, N'Chiếc'),
    
    -- Vật liệu cách nhiệt
    (N'Bông cách nhiệt', 6, 50000, 150, N'M2'),
    (N'Tấm xốp cách nhiệt', 6, 20000, 100, N'M2'),
    (N'Tấm kim loại cách nhiệt', 6, 300000, 50, N'M2'),
    (N'Gạch cách nhiệt', 6, 25000, 80, N'M2');
GO

-- Thêm dữ liệu vào bảng dbo.NhanVien
INSERT INTO dbo.NhanVien (TenNhanVien, SoDienThoai, DiaChi) VALUES
    (N'Đặng Bá Đức', '0123456789', N'Lê Trọng Tấn, Tân Phú'),
    (N'Lê Hồng Thắm', '0987654321', N'Phạm Văn Đồng, Thủ Đức'),
    (N'Trần Minh Hoàng', '0912345678', N'Nguyễn Thái Bình, Q1'),
    (N'Nguyễn Thị Bích', '0934567890', N'Nguyễn Văn Cừ, Q5'),
    (N'Phạm Văn Nam', '0909876543', N'Đường 3/2, Q10'),
    (N'Lê Văn An', '0922334455', N'Triệu Thị Trinh, Q2'),
    (N'Tô Văn Lộc', '0945678901', N'Trần Hưng Đạo, Q3'),
    (N'Nguyễn Thanh Hải', '0956789012', N'Nguyễn Hữu Cảnh, Q4'),
    (N'Trần Thị Kim', '0967890123', N'Tôn Đức Thắng, Q7'),
    (N'Nguyễn Đình Chiến', '0978901234', N'Hồ Văn Huê, Q9');
GO

-- Thêm dữ liệu vào bảng dbo.NhaCungCap
INSERT INTO dbo.NhaCungCap (TenNhaCungCap, SoDienThoai, DiaChi) VALUES
    (N'Công ty Vật liệu Xây dựng ABC', '0147258369', N'606 Đường V, Quận W, Thành phố X'),
    (N'Công ty Cổ phần Điện tử XYZ', '0741852963', N'707 Đường Y, Quận Z, Thành phố A'),
    (N'Công ty TNHH Vật tư xây dựng DEF', '0123456789', N'123 Đường A, Quận B, Thành phố C'),
    (N'Công ty TNHH Thương mại GHI', '0987654321', N'234 Đường D, Quận E, Thành phố F'),
    (N'Công ty Vật liệu xây dựng JKL', '0234567890', N'345 Đường G, Quận H, Thành phố I'),
    (N'Công ty CP Vật liệu xây dựng MNO', '0345678901', N'456 Đường K, Quận L, Thành phố M'),
    (N'Công ty TNHH Sản xuất PQR', '0456789012', N'567 Đường N, Quận O, Thành phố P'),
    (N'Công ty Cổ phần STU', '0567890123', N'678 Đường Q, Quận R, Thành phố S'),
    (N'Công ty TNHH Vật tư xây dựng VWX', '0678901234', N'789 Đường T, Quận U, Thành phố V'),
    (N'Công ty Vật liệu xây dựng YZ', '0789012345', N'890 Đường X, Quận Y, Thành phố Z');
GO

-- Thêm dữ liệu vào bảng dbo.KhachHang
INSERT INTO dbo.KhachHang (TenKhachHang, SoDienThoai, DiaChi) VALUES
    (N'Nguyễn Văn A', '0123456789', N'123 Đường ABC, Hà Nội'),
    (N'Trần Thị B', '0987654321', N'456 Đường DEF, TP.HCM'),
    (N'Phạm Minh C', '0912345678', N'789 Đường GHI, Đà Nẵng'),
    (N'Lê Thị D', '0934567890', N'321 Đường JKL, Hải Phòng'),
    (N'Trần Văn E', '0909876543', N'654 Đường MNO, Nha Trang'),
    (N'Nguyễn Thị F', '0922334455', N'987 Đường PQR, Bình Dương'),
    (N'Tô Văn G', '0945678901', N'543 Đường STU, Vũng Tàu'),
    (N'Nguyễn Thanh H', '0956789012', N'876 Đường VWX, Cần Thơ'),
    (N'Trần Thị I', '0967890123', N'234 Đường YZ, Đồng Nai'),
    (N'Nguyễn Đình J', '0978901234', N'567 Đường AB, Long An');
GO

-- Thêm dữ liệu vào bảng dbo.PhieuNhap
INSERT INTO dbo.PhieuNhap (MaNhaCungCap, MaNhanVien, NgayNhap, TongTien, TrangThai) VALUES
    (1, 1, '2024-10-01 08:30:00', 6500000, N'Chờ xử lý'),
    (2, 2, '2024-10-02 09:45:00', 13000000, N'Đang xử lý'),
    (3, 3, '2024-10-05 14:00:00', 16500000, N'Đã xử lý'),
    (4, 4, '2024-10-07 16:20:00', 7500000, N'Đã huỷ'),
    (5, 5, '2024-10-10 10:15:00', 12000000, N'Chờ xử lý'),
    (6, 6, '2024-10-12 11:30:00', 4500000, N'Đang xử lý');
GO

-- Thêm dữ liệu vào bảng dbo.PhieuXuat
INSERT INTO dbo.PhieuXuat (MaKhachHang, MaNhanVien, NgayXuat, TongTien, TrangThai) VALUES
    (1, 1, '2024-10-01 08:30:00', 3250000, N'Chờ xử lý'),
    (2, 2, '2024-10-03 09:45:00', 7400000, N'Đang xử lý'),
    (3, 3, '2024-10-04 14:00:00', 25500000, N'Đã xử lý'),
    (4, 4, '2024-10-06 16:20:00', 2500000, N'Đã huỷ'),
    (5, 5, '2024-10-07 10:15:00', 11000000, N'Chờ xử lý'),
    (6, 6, '2024-10-09 11:30:00', 6000000, N'Đang xử lý');
GO

-- Thêm dữ liệu vào bảng dbo.ChiTietPhieuNhap
INSERT INTO dbo.ChiTietPhieuNhap (MaPhieuNhap, MaSanPham, DonGia, SoLuong) VALUES
    (1, 1, 200000, 25),
    (1, 2, 15000, 100),
    (2, 3, 30000, 100),
    (2, 4, 5000000, 2),
    (3, 5, 1500000, 10),
    (3, 6, 300000, 5),
    (4, 7, 500000, 15),
    (5, 8, 7000000, 1),
    (5, 9, 500000, 10),
    (6, 10, 1500000, 3);
GO

-- Thêm dữ liệu vào bảng dbo.ChiTietPhieuXuat
INSERT INTO dbo.ChiTietPhieuXuat (MaPhieuXuat, MaSanPham, DonGia, SoLuong) VALUES
    (1, 1, 250000, 10),
    (1, 2, 15000, 50),
    (2, 3, 30000, 80),
    (2, 4, 5000000, 1),
    (3, 5, 1500000, 15),
    (3, 6, 300000, 10),
    (4, 7, 500000, 5),
    (5, 8, 7000000, 1),
    (5, 9, 500000, 8),
    (6, 10, 1500000, 4);
GO

-- Thêm dữ liệu vào bảng dbo.TaiKhoan
INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, VaiTro, MaNhanVien) VALUES
    ('admin', '123', 0, 1),
    ('user1', '123', 1, 2),
    ('user2', '123', 2, 3),
	('user3', '123', 3, 4),
    ('user4', '123', 4, 5);
GO