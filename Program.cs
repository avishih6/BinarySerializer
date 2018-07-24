using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace BinarySerializationExercise {
    [DataContract(IsReference=true)]
    class Person {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public Person Spause { get; set; }
    }

    public static class Serializer {
        public static void Serialize<T> (T root, Stream stm) where T : class 
        {         
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stm, root);
        }

        public static T Deserialize<T> (Stream stm) where T : class {
            var serializer = new DataContractSerializer(typeof(T));
            var obj = serializer.ReadObject(stm);

            return (T)obj;
        }
    }

    class Program {
        static void Main (string[] args) {
            var p1 = new Person { Name = "Homer", Age = 40 };
            var p2 = new Person { Name = "Marge", Age = 30 };
            p1.Spause = p2;
            p2.Spause = p1;
            using (var ms = new MemoryStream ()) {
                Serializer.Serialize (p1, ms);
                ms.Position = 0;
                var p3 = Serializer.Deserialize<Person> (ms);
                Debug.Assert (p3.Name == "Homer");
                Debug.Assert (p3.Age == 40);
                var p4 = p3.Spause;
                Debug.Assert (p4.Spause == p3);
                Debug.Assert (p4.Age == 30);
            }
        }
    }
}