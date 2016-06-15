using UnityEngine;
public class FRandom
{
        public int[] chances;//массив индексов

        public FRandom(int[] c){chances = c;}

        public int Get()
        {
                int all = 0;//общее число шансов
                foreach(int c in chances)all+=c;
                int lr = 0;//начало отсчета от предыдущего шанса
                int r = Random.Range(0, all);//берется случайное число из общего количества шансов
                for(int i = 0; i < chances.Length; i++)
                {
                        if(chances[i] > 0)//если имеется шанс на выпадение индекса, идем дальше
                        {
                                if(r >= lr && r < lr + chances[i])//если случайное число находится в границах индекса...
                                {
                                        chances[i]--;//убираем один шанс у индекса
                                        return i;//возвращаем индекс
                                }
                                else lr += chances[i];//или прибавляем шансы от индекса к предыдущим шансам
                        }
                }
                return -1;//если массив индексов пуст или исчерпаны все шансы, возвращаем -1
        }
}
