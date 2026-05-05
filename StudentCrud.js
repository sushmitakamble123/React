import axios from "axios";
import { useEffect, useState } from "react";

const BASE_URL = "http://localhost:5007/api/Student";

function StudentCrud() {

  const [id, setId] = useState("");
  const [name, setName] = useState("");
  const [course, setCourse] = useState("");
  const [students, setStudents] = useState([]);

  useEffect(() => {
    Load();
  }, []);

  async function Load() {
    try {
      const result = await axios.get(`${BASE_URL}`);
      setStudents(result.data);
    } catch (err) {
      console.log(err);
    }
  }

  async function save(e) {
    e.preventDefault();

    if (!name || !course) {
      alert("Name and Course are required");
      return;
    }

    try {
      await axios.post(`${BASE_URL}`, {
        name,
        course
      });

      alert("Student Added");
      setName("");
      setCourse("");
      Load();

    } catch (err) {
      console.log(err.response);
    }
  }

  function editStudent(student) {
    setId(student.id);
    setName(student.name);
    setCourse(student.course);
  }

  async function DeleteStudent(id) {
    try {
      await axios.delete(`${BASE_URL}/${id}`);
      alert("Deleted");
      Load();
    } catch (err) {
      console.log(err);
    }
  }

  async function update(e) {
    e.preventDefault();

    if (!id) {
      alert("Select a student to update");
      return;
    }

    try {
      await axios.patch(`${BASE_URL}/${id}`, {
        id,
        name,
        course
      });

      alert("Updated");
      setId("");
      setName("");
      setCourse("");
      Load();

    } catch (err) {
      console.log(err.response);
    }
  }

  return (
    <div className="container mt-5">

      <h2 className="text-center mb-4">🎓 Student CRUD App</h2>

      {/* Form */}
      <div className="card p-4 shadow">
        <form onSubmit={save}>
          <input type="hidden" value={id} />

          <div className="mb-3">
            <label className="form-label">Name</label>
            <input
              type="text"
              className="form-control"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>

          <div className="mb-3">
            <label className="form-label">Course</label>
            <input
              type="text"
              className="form-control"
              value={course}
              onChange={(e) => setCourse(e.target.value)}
            />
          </div>

          <button type="submit" className="btn btn-success me-2">
            Add
          </button>

          <button type="button" className="btn btn-warning" onClick={update}>
            Update
          </button>
        </form>
      </div>

      {/* Table */}
      <div className="card mt-4 shadow p-3">
        <table className="table table-hover text-center">
          <thead className="table-dark">
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Course</th>
              <th>Action</th>
            </tr>
          </thead>

          <tbody>
            {students.map((student) => (
              <tr key={student.id}>
                <td>{student.id}</td>
                <td>{student.name}</td>
                <td>{student.course}</td>
                <td>
                  <button
                    className="btn btn-warning btn-sm me-2"
                    onClick={() => editStudent(student)}
                  >
                    Edit
                  </button>

                  <button
                    className="btn btn-danger btn-sm"
                    onClick={() => DeleteStudent(student.id)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>

        </table>
      </div>

    </div>
  );
}

export default StudentCrud;