using System;
using Google.Cloud.Firestore;
using System.Threading.Tasks;
using System.Collections.Generic;
using dojo_net.Modelo;


namespace dojo_.net
{
    public class FirebaseAccount
    {
        private readonly static FirebaseAccount _instancia=new FirebaseAccount();
        FirestoreDb _db;
        public FirebaseAccount()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + @"Firebase-SDK.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",path);
            _db=FirestoreDb.Create("dojo-net-core");
            Console.WriteLine("Se conecto correctamente");
        }
        public static FirebaseAccount Instancia
        {
            get
            {
                return _instancia;
            }
        }
        public async Task<String> AddUser(Usuario user)
        {
            DocumentReference coll = _db.Collection("Usuarios").Document();
            Dictionary <String,object> data = new Dictionary<string,object>()
            {
                {"Cedula",user.Cedula},
                {"Nombre",user.Nombre},
                {"Coreo",user.Correo},
                {"Carrera",user.Carrera}
            };
            await coll.SetAsync(data);
            return "Usuario guardado con Id"+coll.Id;

        }

        public async Task<List<Usuario>> GetUser()
        {
            CollectionReference userRef = _db.Collection("Usuarios");
            QuerySnapshot queryUser = await userRef.GetSnapshotAsync();
            List<Usuario> userList = new List<Usuario>();
            foreach(DocumentSnapshot documentSnapshot in queryUser.Documents)
            {
                Dictionary <String,Object> usuario = documentSnapshot.ToDictionary();
                Usuario user = new Usuario();
                foreach(var item in usuario)
                {
                    if (item.Key == "Nombre")
                    {
                        user.Nombre=(String)item.Value;
                    }else if (item.Key=="Cedula")
                    {
                        user.Cedula=(String) item.Value;
                    }else if (item.Key=="Correo")
                    {
                        user.Correo=(String) item.Value;
                    }else if (item.Key=="Carrera")
                    {
                        user.Carrera=(String) item.Value;
                    }
                }
                userList.Add(user);
            }
            return userList;
        }
        
        public async Task<String> DeleteUser (String id)
        {
            DocumentReference userDelete = _db.Collection("Usuarios").Document(id);
            await userDelete.DeleteAsync();
            return "Usuario con Id"+id+"eliminado";
        }
        
    }
}