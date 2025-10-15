namespace ProductService.Models;

/// <summary>
/// 产品实体
/// 使用 C# Record 类型定义领域模型
/// </summary>
/// 
/// 属性说明：
///   - Id (int): 产品唯一标识符，数据库主键
///   - Name (string): 产品名称
///   - Description (string): 产品描述
///   - Price (decimal): 产品价格，使用 decimal 类型保证精度
public record Product(int Id, string Name, string Description, decimal Price);
