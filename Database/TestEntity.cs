using EntityFrameworkExtensionTest.Enum;

namespace EntityFrameworkExtensionTest.Database
{
    class TestEntity
    {
        public int Id { get; set; }
        public TestEnum TheEnum { get; set; }
    }
}
