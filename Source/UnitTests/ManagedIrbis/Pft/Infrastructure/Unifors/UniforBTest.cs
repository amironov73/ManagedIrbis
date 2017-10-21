using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforBTest
    {
        private void _B
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforB_Convolution_1()
        {
            _B("B", "");
            _B("BУ попа была собака", "СБПУОЫОБЛПАААКА");
            _B("BEat the hit!", "HTEIHATET");
            _B("B+-*=", "");
            _B("B[Вот] /новый/ <поворот>", "ПНВОООВВТОЫРЙОТ");
            _B("Bдесятирублируйте", "ДЕСЯТИРУБЛИРУЙТЕ");
            _B("Bого-го", "ГООГО");

            // Очень длинная строка
            _B("BОднажды весною, в час небывало жаркого заката, в Москве, на Патриарших прудах, появились два гражданина. Первый из них, одетый в летнюю серенькую пару, был маленького роста, упитан, лыс, свою приличную шляпу пирожком нес в руке, а на хорошо выбритом лице его помещались сверхъестественных размеров очки в черной роговой оправе. Второй − плечистый, рыжеватый, вихрастый молодой человек в заломленной на затылок клетчатой кепке − был в ковбойке, жеваных белых брюках и в черных тапочках.", "ТЧВИББЖКВБККЗНЗВЧМВРПВОРЧВОРСПЕЛВХНАРВНПШПСЛУРМБПСЛВОНИПГДПППНМВ");

            // На сладкое
            _B("BΠλάτων Σωκράτης Ἀριστοτέλης", "ἈΣΠΡΩΛΙΚΆΣΡΤΤΆΩΟΤΝΤΗΈςΛΗς");
        }
    }
}
