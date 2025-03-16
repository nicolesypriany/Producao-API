import { pathRoutes } from "@/routers";
import { useNavigate } from "react-router";

const Footer = () => {
  const navigate = useNavigate();

  return (
    <>Footer</>
    // <Flex justifyContent="flex-end" gap={3}>
    //   <Button type="Submit" w="25%">
    //     Cadastrar
    //   </Button>
    //   <Button
    //     w="25%"
    //     colorPalette="gray"
    //     variant="surface"
    //     onClick={() => navigate(routes.home)}
    //   >
    //     Voltar
    //   </Button>
    // </Flex>
  );
};

export default Footer;
