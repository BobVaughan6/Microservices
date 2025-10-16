namespace UserService.Models;

/// <summary>
/// 用户实体
/// 使用 C# 9.0 Record 类型定义不可变数据对象
/// </summary>
/// 
/// Record 特点：
///   1. 不可变（Immutable）：创建后不能修改，保证数据一致性
///   2. 值相等性：基于属性值比较，而不是引用
///   3. 简洁语法：自动生成构造函数、ToString、Equals、GetHashCode
///   4. With 表达式：可以创建修改了部分属性的新副本
/// 
/// 属性说明：
///   - Id: 用户唯一标识符（主键）
///   - Name: 用户姓名
///   - Email: 用户邮箱地址
public record User(int Id, string Name, string Email);
