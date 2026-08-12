import axios from "axios";

export const getUserDetails = async (userId, token) => {
    const response = await axios.get(
        `https://localhost:44389/api/UserDetail/GetById/${userId}`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    return response.data;
};

export default getUserDetails