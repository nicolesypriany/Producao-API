import { Button, Flex, Modal, TextInput, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";

const Header = ({ children }) => {
  return (
    <Flex w="100%" mb={10} align="center" justify="space-between">
      <Title fontSize="4xl">{children}</Title>
      <Button variant="light">Adicionar máquina</Button>
    </Flex>
  );
};

export default Header;

const ModalAddItem = () => {
  const [opened, { open, close }] = useDisclosure(false);

  return (
    <>
      <Modal opened={opened} onClose={close} title="Authentication" centered>
        <Flex direction="column" gap={4}>
          <TextInput
            label="Nome da máquina"
            placeholder="Digite o nome da máquina"
            {...register("machineName", {
              required: "Nome da máquina é obrigatório",
            })}
          />

          <TextInput
            label="Marca"
            placeholder="Digite o nome da marca"
            {...register("brand", {
              required: "Marca é obrigatório",
            })}
          />
        </Flex>
      </Modal>

      <Button variant="default" onClick={open}>
        Open centered Modal
      </Button>
    </>
  );
};
