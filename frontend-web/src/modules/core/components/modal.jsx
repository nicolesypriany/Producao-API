import {
  Button,
  Group,
  Modal,
  ModalStack,
  Text,
  Tooltip,
  useModalsStack,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";

const RegisterModal = ({ children, title, registerType }) => {
  const stack = useModalsStack(["register-item", "cancel-confirm-action"]);
  const confirmTitle = `Deseja cancelar o cadastro ${registerType}?`;

  return (
    <ModalStack>
      <Modal centered title={title} {...stack.register("register-item")}>
        {children}
        <Group mt="lg" justify="flex-end">
          <Button
            w="30%"
            variant="filled"
            onClick={() => stack.open("cancel-confirm-action")}
          >
            Cancelar
          </Button>
          <Button type="submit" w="30%" variant="filled">
            Salvar
          </Button>
        </Group>
      </Modal>

      <Modal
        centered
        title={confirmTitle}
        {...stack.register("cancel-confirm-action")}
      >
        Tem certeza que deseja cancelar o cadastro de {registerType}? Todas as
        informações serão perdidas.
        <Group mt="lg" justify="flex-end">
          <Button
            w="30%"
            variant="filled"
            onClick={() => stack.close("cancel-confirm-action")}
          >
            Cancelar
          </Button>
          <Button variant="filled" w="30%" onClick={stack.closeAll}>
            Confirmar
          </Button>
        </Group>
      </Modal>

      <Button
        w="18%"
        size="sm"
        variant="filled"
        onClick={() => stack.open("register-item")}
      >
        <Text truncate>{title}</Text>
      </Button>
    </ModalStack>
  );
};

export default RegisterModal;
