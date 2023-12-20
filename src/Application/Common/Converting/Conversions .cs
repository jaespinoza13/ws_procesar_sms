
using Application.Common.Models;

namespace Application.Common.Converting
{
    public static class Conversions
    {
        #region Métodos "Conversión de Conjunto de Datos a un objeto/Lista de una Clase"
        /// <summary>
        /// Convierte un Conjunto de datos a una lista de una Clase específica
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <returns></returns>
        public static List<T> ConvertConjuntoDatosToListClass<T>(ConjuntoDatos conjuntoDatos)
        {
            List<T> lst_array = new();

            foreach (var item in conjuntoDatos.lst_tablas[0].lst_filas!)
            {
                T obj = (T)Converting.MapDictToObj(item.nombre_valor, typeof(T));
                lst_array.Add(obj);
            }

            return lst_array;
        }


        /// <summary>
        /// Convierte un Conjunto de datos a un objeto de una Clase específica
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <returns></returns>
        public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos)
        {

            T? obj = default(T);
            foreach (var item in conjuntoDatos.lst_tablas[0].lst_filas!)
            {
                obj = (T)Converting.MapDictToObj(item.nombre_valor, typeof(T));
            }

            return obj!;
        }

        public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos, int posicion)
        {

            T? obj = default( T );
            foreach (var item in conjuntoDatos.lst_tablas[posicion].lst_filas!)
            {
                obj = (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) );
            }

            return obj!;
        }
        /// <summary>
        /// Convierte un Conjunto de datos a una lista de una Clase específica se puede enviar la tabla(0,1..) a convertir
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertConjuntoDatosToListClassDynamic<T>(ConjuntoDatos cuerpo, int table)
        {
            return cuerpo.lst_tablas[table].lst_filas!.Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) ) ).ToList();
        }

        #endregion
    }
}
